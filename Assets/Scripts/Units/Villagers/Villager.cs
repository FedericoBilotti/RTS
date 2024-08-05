using System.Collections.Generic;
using Manager;
using Player;
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
        public IWork PreviousWork { get; private set; }

        private void Start()
        {
            foreach (ResourcesManager.ResourceType item in System.Enum.GetValues(typeof(ResourcesManager.ResourceType)))
            {
                _inventoryResources[item] = 0;
            }

            CreateFSM();
        }

        private void OnEnable() => UnitManager.Instance.AddVillager(this);
        private void OnDisable() => UnitManager.Instance.RemoveVillager(this);

        private void Update() => fsm?.Update();
        private void FixedUpdate() => fsm?.FixedUpdate();

        public void SetStateName(string state) => _actualState = state;

        public void SetStorage(IStorage center) => ActualStorage = center;

        public void SetWork(IWork work)
        {
            if (ActualWork != null) 
                SetPreviousWork(ActualWork);
            
            ActualWork = work;
        }
        
        public void SetPreviousWork(IWork work) => PreviousWork = work;

        #region Resources Storage Methods

        public void AddResourcesToCenter()
        {
            foreach (ResourcesManager.ResourceType resourceType in System.Enum.GetValues(typeof(ResourcesManager.ResourceType)))
            {
                ResourcesManager.Instance.AddResourceAmount(resourceType, _inventoryResources[resourceType]);
                RemoveResourceFromInventory(resourceType);
            }
        }

        public void AddResourceToStorage(ResourcesManager.ResourceType resourceType)
        {
            ResourcesManager.Instance.AddResourceAmount(resourceType, _inventoryResources[resourceType]);
            RemoveResourceFromInventory(resourceType);
        }
        
        #endregion

        #region Villager Inventory Methods

        public bool HasResourceInInventory(ResourcesManager.ResourceType resourceType) => _inventoryResources[resourceType] > 0;

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

        #region FSM

        private void CreateFSM()
        {
            fsm = new FiniteStateMachine();

            var idle = new Idle(this, agent, _villagerSO);
            var moving = new Moving(this, agent, _villagerSO);
            var moveToResource = new MoveToResource(this); // Deberia llamarse MoveToWork
            var workVillager = new WorkVillager(this);
            var moveToStorage = new MoveToStorage(this);
            var searchNewResource = new SearchNewResource(this);

            IdleTransitions(idle, moveToStorage, moveToResource, moving);
            MoveToTransitions(moving, idle, moveToResource, moveToStorage);
            MoveToResourceTransitions(moveToResource, workVillager, moveToStorage, moving);
            WorkVillagerTransitions(workVillager, moveToResource, moving, searchNewResource, moveToStorage);
            MoveToStorageTransitions(moveToStorage, moveToResource, searchNewResource, idle);
            SearchNewResourceTransitions(searchNewResource, moveToStorage, moveToResource, idle);

            fsm.SetState(idle);
        }

        private void IdleTransitions(Idle idle, MoveToStorage moveToStorage, MoveToResource moveToResource, Moving moving)
        {
            fsm.AddTransition(idle, moving, new FuncPredicate(() => ActualWork == null && !agent.isStopped));
            fsm.AddTransition(idle, moveToResource, new FuncPredicate(() => ActualWork != null && !ResourceIsEmpty()));
            fsm.AddTransition(idle, moveToStorage, new FuncPredicate(() => ActualStorage != null)); // && !IsInventoryEmpty()
        }

        private void MoveToTransitions(Moving moving, Idle idle, MoveToResource moveToResource, MoveToStorage moveToStorage)
        {
            fsm.AddTransition(moving, idle, new FuncPredicate(() => ActualWork == null && !agent.hasPath));
            fsm.AddTransition(moving, moveToResource, new FuncPredicate(() => ActualWork != null && !ResourceIsEmpty()));
            fsm.AddTransition(moving, moveToStorage, new FuncPredicate(() => ActualStorage != null && !IsInventoryEmpty()));
        }

        private void MoveToResourceTransitions(MoveToResource moveToResource, WorkVillager workVillager, MoveToStorage moveToStorage, Moving moving)
        {
            fsm.AddTransition(moveToResource, moveToStorage, new FuncPredicate(() => ActualStorage != null && ActualWork == null));
            fsm.AddTransition(moveToResource, moving, new FuncPredicate(() => ActualWork == null));
            fsm.AddTransition(moveToResource, workVillager, new FuncPredicate(() => MoveToResource(ActualWork.GetResourceSO().ResourceType)));
        }

        private void WorkVillagerTransitions(WorkVillager workVillager, MoveToResource moveToResource, Moving moving, SearchNewResource searchNewResource,
                MoveToStorage moveToStorage)
        {
            fsm.AddTransition(workVillager, moving, new FuncPredicate(() => ActualWork == null));
            fsm.AddTransition(workVillager, searchNewResource, new FuncPredicate(() => ResourceIsEmpty() && !IsInventoryFull(ActualWork.GetResourceSO().ResourceType)));
            fsm.AddTransition(workVillager, moveToStorage, new FuncPredicate(() => ActualStorage != null && IsInventoryFull(ActualWork.GetResourceSO().ResourceType)));
            fsm.AddTransition(workVillager, moveToResource, new FuncPredicate(() => PreviousWork != null && PreviousWork != ActualWork));
        }

        private void MoveToStorageTransitions(MoveToStorage moveToStorage, MoveToResource moveToResource, SearchNewResource searchNewResource, Idle idle)
        {
            fsm.AddTransition(moveToStorage, moveToResource, new FuncPredicate(() => ActualWork != null && !ResourceIsEmpty() && !IsInventoryFull(ActualWork.GetResourceSO().ResourceType)));
            fsm.AddTransition(moveToStorage, searchNewResource, new FuncPredicate(() => ActualWork != null && ResourceIsEmpty() && !IsInventoryFull(ActualWork.GetResourceSO().ResourceType)));
            fsm.AddTransition(moveToStorage, idle, new FuncPredicate(() => ActualStorage == null || IsInventoryEmpty()));
        }

        private void SearchNewResourceTransitions(SearchNewResource searchNewResource, MoveToStorage moveToStorage, MoveToResource moveToResource, Idle idle)
        {
            fsm.AddTransition(searchNewResource, moveToResource, new FuncPredicate(() => ActualWork != null && !ResourceIsEmpty()));
            fsm.AddTransition(searchNewResource, moveToStorage, new FuncPredicate(() => ActualWork == null && !IsInventoryEmpty()));
            fsm.AddTransition(searchNewResource, idle, new FuncPredicate(() => ActualWork == null));
        }

        private bool MoveToResource(ResourcesManager.ResourceType resourceType) => ActualWork.GetResourceSO().ResourceType == resourceType && IsNearResource();
        private bool IsNearResource() => Vector3.Distance(transform.position, ActualWork.Position) < 2.5f;
        private bool ResourceIsEmpty() => ActualWork.GetActualAmount() <= 0;

        #endregion
    }
}