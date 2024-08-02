using System.Collections.Generic;
using Manager;
using StateMachine;
using Structures.Storages;
using Units.Resources;
using Units.SO;
using Units.Villagers.States;
using UnityEngine;

namespace Units.Villagers
{
    public class Villager : Unit
    {
        [SerializeField] private string _actualState; // Para saber el estado en que me encuentro
        [SerializeField] private VillagerSO _villagerSO;

        private readonly Dictionary<ResourcesManager.ResourceType, int> _inventoryResources = new();

        public IStorage ActualStorage { get; private set; }
        public IWork ActualWork { get; private set; }

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

            var idle = new Idle(this, agent, _villagerSO);
            var moving = new Moving(this, agent, _villagerSO);
            var moveToResource = new MoveToResource(this); // Deberia llamarse MoveToWork

            var workVillager = new WorkVillager(this);
            // var mining = new Mining(this);
            // var chopping = new Chop(this);

            var moveToStorage = new MoveToStorage(this);
            var searchNewResource = new SearchNewResource(this);

            IdleTransitions(idle, moveToStorage, moveToResource, moving);
            MoveToTransitions(moving, idle, moveToResource, moveToStorage);
            MoveToResourceTransitions(moveToResource, workVillager,moveToStorage, moving);
            WorkVillagerTransitions(workVillager, moving, searchNewResource, moveToStorage);
            // MiningTransitions(mining, chopping, moving, searchNewResource, moveToStorage);
            // ChopTransitions(chopping, mining, moving, searchNewResource, moveToStorage);
            // FoodTransitions(chopping, mining, moving, searchNewResource, moveToStorage);
            MoveToStorageTransitions(moveToStorage, moveToResource, searchNewResource, idle);
            SearchNewResourceTransitions(searchNewResource, moveToStorage, moveToResource, idle);

            fsm.SetState(idle);
        }

        private void IdleTransitions(Idle idle, MoveToStorage moveToStorage, MoveToResource moveToResource, Moving moving)
        {
            fsm.AddTransition(idle, moveToStorage, new FuncPredicate(() => ActualStorage != null && !IsInventoryEmpty()));
            fsm.AddTransition(idle, moveToResource, new FuncPredicate(() => ActualWork != null && !ResourceIsEmpty()));
            fsm.AddTransition(idle, moving, new FuncPredicate(() => ActualWork == null && agent.hasPath));
        }

        private void MoveToTransitions(Moving moving, Idle idle, MoveToResource moveToResource, MoveToStorage moveToStorage)
        {
            fsm.AddTransition(moving, idle, new FuncPredicate(() => ActualWork == null && (!agent.hasPath || agent.isStopped)));
            fsm.AddTransition(moving, moveToResource, new FuncPredicate(() => ActualWork != null && !ResourceIsEmpty()));
            fsm.AddTransition(moving, moveToStorage, new FuncPredicate(() => ActualStorage != null && !IsInventoryEmpty()));
        }

        private void MoveToResourceTransitions(MoveToResource moveToResource, WorkVillager workVillager, MoveToStorage moveToStorage, Moving moving)
        {
            fsm.AddTransition(moveToResource, moveToStorage, new FuncPredicate(() => ActualStorage != null && ActualWork == null));
            fsm.AddTransition(moveToResource, moving, new FuncPredicate(() => ActualWork == null));
            fsm.AddTransition(moveToResource, workVillager, new FuncPredicate(() => MoveToResource(ActualWork.GetResourceSO().ResourceType)));
            // fsm.AddTransition(moveToResource, mining, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Gold)));
            // fsm.AddTransition(moveToResource, food, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Food)));
        }

        // private void MiningTransitions(Mining mining, Chop chopping, Moving moving, SearchNewResource searchNewResource, MoveToStorage moveToStorage)
        // {
        //     fsm.AddTransition(mining, moving, new FuncPredicate(() => !ActualResource));
        //     fsm.AddTransition(mining, searchNewResource, new FuncPredicate(() => ResourceIsEmpty() && !IsInventoryFull(ResourcesManager.ResourceType.Gold)));
        //     fsm.AddTransition(mining, moveToStorage, new FuncPredicate(() => IsInventoryFull(ResourcesManager.ResourceType.Gold)));
        //     fsm.AddTransition(mining, chopping, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Wood)));
        // }
        //
        // private void ChopTransitions(Chop chopping, Mining mining, Moving moving, SearchNewResource searchNewResource, MoveToStorage moveToStorage)
        // {
        //     fsm.AddTransition(chopping, moving, new FuncPredicate(() => !ActualResource));
        //     fsm.AddTransition(chopping, searchNewResource, new FuncPredicate(() => ResourceIsEmpty() && !IsInventoryFull(ResourcesManager.ResourceType.Wood)));
        //     fsm.AddTransition(chopping, moveToStorage, new FuncPredicate(() => IsInventoryFull(ResourcesManager.ResourceType.Wood)));
        //     fsm.AddTransition(chopping, mining, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Gold)));
        // }
        //
        // private void FoodTransitions(Chop chopping, Mining mining, Moving moving, SearchNewResource searchNewResource, MoveToStorage moveToStorage)
        // {
        //     // fsm.AddTransition(food, moving, new FuncPredicate(() => !_actualResource));
        //     // fsm.AddTransition(food, searchNewResource, new FuncPredicate(() => ResourceIsEmpty() && !IsInventoryFull(ResourcesManager.ResourceType.Food)));
        //     // fsm.AddTransition(food, moveToStorage, new FuncPredicate(() => IsInventoryFull(ResourcesManager.ResourceType.Food)));
        //     // fsm.AddTransition(food, mining, new FuncPredicate(() => MoveToResource(ResourcesManager.ResourceType.Food) && !Equals(_previousResource, ActualResource)));
        // }

        private void WorkVillagerTransitions(WorkVillager workVillager, Moving moving, SearchNewResource searchNewResource, MoveToStorage moveToStorage)
        {
            fsm.AddTransition(workVillager, moving, new FuncPredicate(() => ActualWork == null));
            fsm.AddTransition(workVillager, searchNewResource, new FuncPredicate(() => ResourceIsEmpty() && !IsInventoryFull(ActualWork.GetResourceSO().ResourceType)));
            fsm.AddTransition(workVillager, moveToStorage, new FuncPredicate(() => IsInventoryFull(ActualWork.GetResourceSO().ResourceType)));
        }

        private void MoveToStorageTransitions(MoveToStorage moveToStorage, MoveToResource moveToResource, SearchNewResource searchNewResource, Idle idle)
        {
            fsm.AddTransition(moveToStorage, moveToResource, new FuncPredicate(() => ActualWork != null && !ResourceIsEmpty() && !IsInventoryFull(ActualWork.GetResourceSO().ResourceType)));
            fsm.AddTransition(moveToStorage, searchNewResource, new FuncPredicate(() => ActualWork != null && ResourceIsEmpty() && !IsInventoryFull(ActualWork.GetResourceSO().ResourceType)));
            fsm.AddTransition(moveToStorage, idle, new FuncPredicate(IsInventoryEmpty));
        }

        private void SearchNewResourceTransitions(SearchNewResource searchNewResource, MoveToStorage moveToStorage, MoveToResource moveToResource, Idle idle)
        {
            fsm.AddTransition(searchNewResource, moveToResource, new FuncPredicate(() => ActualWork != null && !ResourceIsEmpty()));
            fsm.AddTransition(searchNewResource, moveToStorage, new FuncPredicate(() => ActualWork != null && !IsInventoryEmpty()));
            fsm.AddTransition(searchNewResource, idle, new FuncPredicate(() => ActualWork == null));
        }

        private bool MoveToResource(ResourcesManager.ResourceType resourceType) => ActualWork.GetResourceSO().ResourceType == resourceType && IsNearResource();
        private bool IsNearResource() => Vector3.Distance(transform.position, ActualWork.Position) < 2.5f;
        private bool ResourceIsEmpty() => ActualWork.GetActualAmount() <= 0;

        #endregion

        // Generico -> IStorage
        public void SetStorage(IStorage center) => ActualStorage = center;
        public void SetWork(IWork work) => ActualWork = work;

        // public void SetResource(Resource resource)
        // {
        //     ActualResource = resource;
        //     ActualResource.IsNotNull(() => ActualResourceTransform = resource.transform);
        //     ActualResource.IsNotNull(() => ActualResourceType = resource.GetResourceSO().ResourceType);
        // }

        public void AddResourceToStorage()
        {
            foreach (ResourcesManager.ResourceType resourceType in System.Enum.GetValues(typeof(ResourcesManager.ResourceType)))
            {
                ResourcesManager.Instance.AddResourceAmount(resourceType, _inventoryResources[resourceType]);
                RemoveResourceFromInventory(resourceType);
            }
        }

        public void AddSpecificResourceToStorage(ResourcesManager.ResourceType resourceType)
        {
            ResourcesManager.Instance.AddResourceAmount(resourceType, _inventoryResources[resourceType]);
            RemoveResourceFromInventory(resourceType);
        }

        #region Villager Inventory

        public void AddResourceToInventory(ResourcesManager.ResourceType resourceType, int amount) => _inventoryResources[resourceType] += amount;
        private void RemoveResourceFromInventory(ResourcesManager.ResourceType resourceType) => _inventoryResources[resourceType] = 0;

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
            return _villagerSO.IsInventoryFull(resourceType, _inventoryResources[resourceType]);
        }

        #endregion
    }
}