using System.Collections.Generic;
using Manager;
using StateMachine;
using Structures.Storages;
using Units.Resources;
using Units.SO;
using Units.Villagers.States;
using UnityEngine;
using Utilities;

namespace Units.Villagers
{
    public class Villager : Unit
    {
        [SerializeField] private string _actualState; // Para saber el estado en que me encuentro

        private IStorage _storage;
        private Resource _actualResource;
        private Transform _actualResourceTransform;
        private ResourcesManager.ResourceType _actualResourceType;

        private readonly Dictionary<ResourcesManager.ResourceType, int> _inventoryResources = new();

        private void Start()
        {
            foreach (ResourcesManager.ResourceType item in System.Enum.GetValues(typeof(ResourcesManager.ResourceType)))
            {
                _inventoryResources[item] = 0;
            }

            CreateFSM();
        }

        private void Update() => fsm?.Update();
        private void FixedUpdate() => fsm?.FixedUpdate();

        public void SetStateName(string state) => _actualState = state;

        #region FSM

        private void CreateFSM()
        {
            fsm = new FiniteStateMachine();

            var idle = new Idle(this, agent, unitSo);
            var moving = new Moving(this, agent, unitSo);
            var moveToResource = new MoveToResource(this);
            var mining = new Mining(this);
            var chopping = new Chop(this);
            var moveToStorage = new MoveToStorage(this);
            var searchNewResource = new SearchNewResource(this);

            IdleTransitions(idle, moveToStorage, moveToResource, moving);
            MoveToTransitions(moving, idle, moveToResource, moveToStorage);
            MoveToResourceTransitions(moveToResource, moveToStorage, moving, chopping, mining);
            MiningTransitions(mining, moving, searchNewResource, moveToStorage);
            ChopTransitions(chopping, moving, searchNewResource, moveToStorage);
            FoodTransitions(chopping, moving, searchNewResource, moveToStorage);
            MoveToStorageTransitions(moveToStorage, moveToResource, searchNewResource, idle);
            SearchNewResourceTransitions(searchNewResource, moveToStorage, moveToResource, idle);

            fsm.SetState(idle);
        }

        private void IdleTransitions(Idle idle, MoveToStorage moveToStorage, MoveToResource moveToResource, Moving moving)
        {
            fsm.AddTransition(idle, moveToStorage, new FuncPredicate(() => _storage != null));
            fsm.AddTransition(idle, moveToResource, new FuncPredicate(() => _actualResource && !ResourceIsEmpty()));
            fsm.AddTransition(idle, moving, new FuncPredicate(() => !_actualResource && agent.hasPath));
        }

        private void MoveToTransitions(Moving moving, Idle idle, MoveToResource moveToResource, MoveToStorage moveToStorage)
        {
            fsm.AddTransition(moving, idle, new FuncPredicate(() => !_actualResource && !agent.hasPath));
            fsm.AddTransition(moving, moveToResource, new FuncPredicate(() => _actualResource && !ResourceIsEmpty()));
            fsm.AddTransition(moving, moveToStorage, new FuncPredicate(() => _storage != null));
        }

        private void MoveToResourceTransitions(MoveToResource moveToResource, MoveToStorage moveToStorage, Moving moving, Chop chopping, Mining mining)
        {
            fsm.AddTransition(moveToResource, moveToStorage,
                    new FuncPredicate(() => _storage != null && !_actualResource)); // -> Puedo hacer que cuando clickeo en un storage, hago nulo el recurso actual.
            fsm.AddTransition(moveToResource, moving, new FuncPredicate(() => !_actualResource));
            fsm.AddTransition(moveToResource, chopping, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Wood)));
            fsm.AddTransition(moveToResource, mining, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Gold)));
            // fsm.AddTransition(moveToResource, food, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Food)));
        }

        private void MiningTransitions(Mining mining, Moving moving, SearchNewResource searchNewResource, MoveToStorage moveToStorage)
        {
            fsm.AddTransition(mining, moving, new FuncPredicate(() => !_actualResource));
            fsm.AddTransition(mining, searchNewResource, new FuncPredicate(() => ResourceIsEmpty() && !IsInventoryFull(ResourcesManager.ResourceType.Gold)));
            fsm.AddTransition(mining, moveToStorage, new FuncPredicate(() => IsInventoryFull(ResourcesManager.ResourceType.Gold)));
        }

        private void ChopTransitions(Chop chopping, Moving moving, SearchNewResource searchNewResource, MoveToStorage moveToStorage)
        {
            fsm.AddTransition(chopping, moving, new FuncPredicate(() => !_actualResource));
            fsm.AddTransition(chopping, searchNewResource, new FuncPredicate(() => ResourceIsEmpty() && !IsInventoryFull(ResourcesManager.ResourceType.Wood)));
            fsm.AddTransition(chopping, moveToStorage, new FuncPredicate(() => IsInventoryFull(ResourcesManager.ResourceType.Wood)));
        }

        private void FoodTransitions(Chop chopping, Moving moving, SearchNewResource searchNewResource, MoveToStorage moveToStorage)
        {
            // fsm.AddTransition(food, moveTo, new FuncPredicate(() => !_actualResource));
            // fsm.AddTransition(food, searchNewResource, new FuncPredicate(() => _actualResource.GetActualAmount() <= 0 && !IsInventoryFull(ResourcesManager.ResourceType.Food)));
            // fsm.AddTransition(food, moveToStorage, new FuncPredicate(() => _storage != null || IsInventoryFull(ResourcesManager.ResourceType.Food)));
        }

        private void MoveToStorageTransitions(MoveToStorage moveToStorage, MoveToResource moveToResource, SearchNewResource searchNewResource, Idle moveTo)
        {
            fsm.AddTransition(moveToStorage, moveToResource, new FuncPredicate(() => _actualResource && !ResourceIsEmpty() && !IsInventoryFull(_actualResourceType)));
            fsm.AddTransition(moveToStorage, searchNewResource, new FuncPredicate(() => _actualResource && ResourceIsEmpty() && !IsInventoryFull(_actualResourceType)));
            fsm.AddTransition(moveToStorage, moveTo, new FuncPredicate(IsInventoryEmpty));
        }

        private void SearchNewResourceTransitions(SearchNewResource searchNewResource, MoveToStorage moveToStorage, MoveToResource moveToResource, Idle idle)
        {
            // from searchNewResource
            fsm.AddTransition(searchNewResource, moveToResource, new FuncPredicate(() => _actualResource && !ResourceIsEmpty()));
            fsm.AddTransition(searchNewResource, moveToStorage, new FuncPredicate(() => !_actualResource && !IsInventoryEmpty()));
            fsm.AddTransition(searchNewResource, idle, new FuncPredicate(() => !_actualResource));
        }

        private bool MoveToResource(ResourcesManager.ResourceType resourceType) => _actualResourceType == resourceType && IsNearResource();
        private bool IsNearResource() => Vector3.Distance(transform.position, _actualResourceTransform.position) < 2.5f;
        private bool ResourceIsEmpty() => _actualResource.GetActualAmount() <= 0;

        #endregion

        // Generico -> IStorage
        public void SetStorage(IStorage center) => _storage = center;

        public void SetResource(Resource resource)
        {
            _actualResource = resource;
            _actualResource.IsNotNull(() => _actualResourceType = resource.GetResourceType());
            _actualResource.IsNotNull(() => _actualResourceTransform = resource.transform);
        }

        public IStorage GetStorage() => _storage;
        public Resource GetResource() => _actualResource;
        public Transform GetResourceTransform() => _actualResourceTransform;
        public ResourcesManager.ResourceType GetResourceType() => _actualResourceType;

        // Para aceptar un tipo especifico de recurso, tengo que chequear a q storage estoy yendo y que recurso estaba sacando.
        public void AddResourceToStorage()
        {
            foreach (ResourcesManager.ResourceType resourceType in System.Enum.GetValues(typeof(ResourcesManager.ResourceType)))
            {
                ResourcesManager.Instance.AddResource(resourceType, _inventoryResources[resourceType]);
                _inventoryResources[resourceType] = 0;
            }
        }

        public void AddSpecificResourceToStorage(ResourcesManager.ResourceType resourceType)
        {
            ResourcesManager.Instance.AddResource(resourceType, _inventoryResources[resourceType]);
            _inventoryResources[resourceType] = 0;
        }

        public void AddResourceToInventory(ResourcesManager.ResourceType resourceType, int amount)
        {
            _inventoryResources[resourceType] += amount;
        }

        private bool IsInventoryEmpty()
        {
            foreach (ResourcesManager.ResourceType resourceType in System.Enum.GetValues(typeof(ResourcesManager.ResourceType)))
            {
                if (_inventoryResources[resourceType] > 0) return false;
            }

            return true;
        }

        private bool IsInventoryFull(ResourcesManager.ResourceType resourceType)
        {
            if (unitSo is VillagerSO villagerSo) return villagerSo.IsInventoryFull(resourceType, _inventoryResources[resourceType]);

            return false;
        }
    }
}