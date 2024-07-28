using System;
using System.Collections.Generic;
using Manager;
using StateMachine;
using Units.Resources;
using Units.Villagers.States;
using UnityEngine;
using Utilities;

namespace Units.Villagers
{
    public class Villager : Unit
    {
        [SerializeField] private int _maxAmountResourceToCarryOn = 20;
        [SerializeField] private Resource _actualResource;
        private Transform _resourceTransform;
        private ResourcesManager.ResourceType _actualResourceType;

        [SerializeField] private string _actualState;
        
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
            var moving = new Moving(this);
            
            // from idle
            fsm.AddTransition(idle, moveToResource, new FuncPredicate(() => _actualResource != null && _actualResource.GetActualAmount() > 0));
            
            // from moving
            // fsm.AddAnyTransition(moving, new FuncPredicate(() => !agent.isStopped));
            
            // from moveToResource
            fsm.AddTransition(moveToResource, chopping, new FuncPredicate(MoveToChopping)); 
            fsm.AddTransition(moveToResource, mining, new FuncPredicate(MoveToMining));
            
            // from mining
            fsm.AddTransition(mining, moveToStorage, new FuncPredicate(IsInventoryFull));
            fsm.AddTransition(chopping, searchNewResource, new FuncPredicate(() => _actualResource == null && !IsInventoryFull()));
            
            // from chop
            fsm.AddTransition(chopping, moveToStorage, new FuncPredicate(IsInventoryFull));
            fsm.AddTransition(chopping, searchNewResource, new FuncPredicate(() => _actualResource == null && !IsInventoryFull()));
            
            // from moveToStorage
            fsm.AddTransition(moveToStorage, moveToResource, new FuncPredicate(() => _actualResource != null && _actualResource.GetActualAmount() > 0 && !IsInventoryFull()));
            fsm.AddTransition(moveToStorage, searchNewResource, new FuncPredicate(() => _actualResource == null && !IsInventoryFull()));
            
            // from searchNewResource
            fsm.AddTransition(searchNewResource, moveToResource, new FuncPredicate(() => _actualResource != null && _actualResource.GetActualAmount() > 0));
            fsm.AddTransition(searchNewResource, idle, new FuncPredicate(() => _actualResource == null));
            
            fsm.SetState(idle);
        }

        private bool MoveToChopping() => _actualResource.GetResourceType() == ResourcesManager.ResourceType.Wood && IsNearResource();
        private bool MoveToMining() => _actualResource.GetResourceType() == ResourcesManager.ResourceType.Gold && IsNearResource();
        private bool IsNearResource() => Vector3.Distance(transform.position, _actualResource.transform.position) < 3f;

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
            Debug.Log(_inventoryResources[resourceType]);
        }

        private bool IsInventoryFull()
        {
            var total = 0;
            
            foreach (ResourcesManager.ResourceType item in System.Enum.GetValues(typeof(ResourcesManager.ResourceType)))
            {
                total += _inventoryResources[item];
            }
            
            return total >= _maxAmountResourceToCarryOn;
        }
    }
}