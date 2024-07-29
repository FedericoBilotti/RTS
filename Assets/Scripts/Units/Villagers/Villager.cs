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
        private Transform _resourceTransform;
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

        public void SetName(string state)
        {
            _actualState = state;
        }

        private void CreateFSM()
        {
            fsm = new FiniteStateMachine();

            var idle = new Idle(this);
            var mining = new Mining(this);
            var chopping = new Chop(this);
            var moveToResource = new MoveToResource(this);
            var moveToStorage = new MoveToStorage(this);
            var searchNewResource = new SearchNewResource(this);

            // from idle
            fsm.AddTransition(idle, moveToResource, new FuncPredicate(() => _actualResource != null && _actualResource.GetActualAmount() > 0));
            
            // from moveToResource
            fsm.AddTransition(moveToResource, chopping, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Wood)));
            fsm.AddTransition(moveToResource, mining, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Gold)));
            // fsm.AddTransition(moveToResource, food, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Food)));

            // from mining
            fsm.AddTransition(mining, moveToStorage, new FuncPredicate(() => IsInventoryFull(ResourcesManager.ResourceType.Gold)));
            fsm.AddTransition(mining, searchNewResource, new FuncPredicate(() => _actualResource == null && !IsInventoryFull(ResourcesManager.ResourceType.Gold)));

            // from chop
            fsm.AddTransition(chopping, moveToStorage, new FuncPredicate(() => IsInventoryFull(ResourcesManager.ResourceType.Wood)));
            fsm.AddTransition(chopping, searchNewResource, new FuncPredicate(() => _actualResource == null && !IsInventoryFull(ResourcesManager.ResourceType.Wood)));

            // from moveToStorage
            fsm.AddTransition(moveToStorage, moveToResource, new FuncPredicate(() => _actualResource != null && !ResourceIsEmpty() && !IsInventoryFull(_actualResourceType)));
            fsm.AddTransition(moveToStorage, searchNewResource, new FuncPredicate(() => _actualResource == null && !IsInventoryFull(_actualResourceType)));

            // from searchNewResource
            fsm.AddTransition(searchNewResource, moveToResource, new FuncPredicate(() => _actualResource != null && !ResourceIsEmpty()));
            fsm.AddTransition(searchNewResource, idle, new FuncPredicate(() => _actualResource == null));

            fsm.SetState(idle);
        }

        private bool MoveToResource(ResourcesManager.ResourceType resourceType) => _actualResource.GetResourceType() == resourceType && IsNearResource();
        private bool IsNearResource() => Vector3.Distance(transform.position, _actualResource.transform.position) < 3f;

        private bool ResourceIsEmpty() => _actualResource.GetActualAmount() <= 0;

        public void SetResource(Resource resource)
        {
            _actualResource = resource;
            resource.IsNotNull(() => _actualResourceType = resource.GetResourceType());
            resource.IsNotNull(() => _resourceTransform = resource.transform);
        }

        public Resource GetResource() => _actualResource;
        public Transform GetResourceTransform() => _resourceTransform;
        public ResourcesManager.ResourceType GetResourceType() => _actualResourceType;

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