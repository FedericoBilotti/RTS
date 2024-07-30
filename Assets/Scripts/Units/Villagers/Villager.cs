using System.Collections.Generic;
using Manager;
using StateMachine;
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

        private Resource _actualResource;
        private Transform _previousResourceTransform;
        private ResourcesManager.ResourceType _previousResourceType;

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

        public void SetName(string state)
        {
            _actualState = state;
        }

        #region FSM

        private void CreateFSM()
        {
            fsm = new FiniteStateMachine();

            var idle = new Idle(this);
            var mining = new Mining(this);
            var chopping = new Chop(this);
            var moveToResource = new MoveToResource(this);
            var moveToStorage = new MoveToStorage(this);
            var searchNewResource = new SearchNewResource(this);

            Transitions(idle, moveToResource, chopping, mining, moveToStorage, searchNewResource);

            fsm.SetState(idle);
        }

        private void Transitions(Idle idle, MoveToResource moveToResource, Chop chopping, Mining mining, MoveToStorage moveToStorage, SearchNewResource searchNewResource)
        {
            // from idle
            fsm.AddTransition(idle, moveToResource, new FuncPredicate(() => _actualResource != null && _actualResource.GetActualAmount() > 0));

            // from moveToResource
            fsm.AddTransition(moveToResource, idle, new FuncPredicate(() => _actualResource == null));
            fsm.AddTransition(moveToResource, chopping, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Wood)));
            fsm.AddTransition(moveToResource, mining, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Gold)));
            // fsm.AddTransition(moveToResource, food, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Food)));

            // from mining
            fsm.AddTransition(mining, moveToStorage, new FuncPredicate(() => IsInventoryFull(ResourcesManager.ResourceType.Gold)));
            fsm.AddTransition(mining, searchNewResource, new FuncPredicate(() => _actualResource == null && !IsInventoryFull(ResourcesManager.ResourceType.Gold)));
            fsm.AddTransition(mining, idle, new FuncPredicate(() => _actualResource == null && agent.hasPath));

            // from chop
            fsm.AddTransition(chopping, moveToStorage, new FuncPredicate(() => IsInventoryFull(ResourcesManager.ResourceType.Wood)));
            fsm.AddTransition(chopping, searchNewResource, new FuncPredicate(() => _actualResource == null && !IsInventoryFull(ResourcesManager.ResourceType.Wood)));
            fsm.AddTransition(chopping, idle, new FuncPredicate(() => _actualResource == null && agent.hasPath));
            
            // from food
            // fsm.AddTransition(food, moveToStorage, new FuncPredicate(() => IsInventoryFull(ResourcesManager.ResourceType.Food)));
            // fsm.AddTransition(food, searchNewResource, new FuncPredicate(() => _actualResource == null && !IsInventoryFull(ResourcesManager.ResourceType.Food)));
            // fsm.AddTransition(food, idle, new FuncPredicate(() => _actualResource == null));

            // from moveToStorage
            fsm.AddTransition(moveToStorage, moveToResource, new FuncPredicate(() => _actualResource != null && !ResourceIsEmpty() && !IsInventoryFull(_previousResourceType)));
            fsm.AddTransition(moveToStorage, searchNewResource, new FuncPredicate(() => _actualResource == null && !IsInventoryFull(_previousResourceType)));

            // from searchNewResource
            fsm.AddTransition(searchNewResource, moveToResource, new FuncPredicate(() => _actualResource != null && !ResourceIsEmpty()));
            fsm.AddTransition(searchNewResource, idle, new FuncPredicate(() => _actualResource == null));
        }

        private bool MoveToResource(ResourcesManager.ResourceType resourceType) => _actualResource.GetResourceType() == resourceType && IsNearResource();
        private bool IsNearResource() => Vector3.Distance(transform.position, _actualResource.transform.position) < 3f;

        private bool ResourceIsEmpty() => _actualResource.GetActualAmount() <= 0;

        #endregion

        public void SetResource(Resource resource)
        {
            _actualResource = resource;
            resource.IsNotNull(() => _previousResourceType = resource.GetResourceType());
            resource.IsNotNull(() => _previousResourceTransform = resource.transform);
        }

        public Resource GetResource() => _actualResource;
        public Transform GetResourceTransform() => _previousResourceTransform;
        public ResourcesManager.ResourceType GetResourceType() => _previousResourceType;

        // Para aceptar un tipo especifico de recurso, tengo que chequear a q storage estoy yendo y que recurso estaba sacando.
        public void AddResourceToStorage()
        {
            foreach (ResourcesManager.ResourceType item in System.Enum.GetValues(typeof(ResourcesManager.ResourceType)))
            {
                ResourcesManager.Instance.AddResource(item, _inventoryResources[item]);
                _inventoryResources[item] = 0;
            }
        }

        public void AddResourceToInventory(ResourcesManager.ResourceType resourceType, int amount)
        {
            _inventoryResources[resourceType] += amount;
        }

        private bool IsInventoryFull(ResourcesManager.ResourceType resourceType)
        {
            if (unitSo is VillagerSO villagerSo) return villagerSo.IsInventoryFull(resourceType, _inventoryResources);

            return false;
        }
    }
}