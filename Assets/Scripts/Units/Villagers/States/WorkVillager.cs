using Manager;
using Player;
using Units.Resources;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Units.Villagers.States
{
    public class WorkVillager : BaseStateVillager
    {
        private IWork _work;

        private readonly CountdownTimer _timer = new(2f);
        private readonly NavMeshAgent _agent;

        private ResourcesManager.ResourceType _resourceType;

        public WorkVillager(Villager villager, NavMeshAgent agent) : base(villager)
        {
            _agent = agent;
        }

        public override void OnEnter()
        {
            _work = villager.ActualWork;
            _resourceType = _work.GetResourceSO().ResourceType;
            
            villager.SetStorage(GameManager.Instance.NearStorage(villager, villager.GetFaction(), _resourceType));
            villager.SetResourceType(villager.ActualWork.GetResourceSO().ResourceType);
            villager.StopMovement();
            
            _timer.Reset(_work.GetResourceSO().TimeToGiveResource);
            _timer.onTimerStop += AddResource;
            _timer.onTimerStop += StartTimer;
            _timer.Start();

            _work.PlayAnimation(villager);

            villager.SetStateName($"Working: {_resourceType}");
        }

        // Play mining animation
        public override void OnUpdate()
        {
            _timer.Tick(Time.deltaTime);
        }

        public override void OnExit()
        {
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            
            _timer.onTimerStop -= AddResource;
            _timer.onTimerStop -= StartTimer;
            _timer.Stop();
        }

        private void AddResource() => villager.AddResourceToInventory(_resourceType, _work.ProvideResource());
        private void StartTimer() => villager.ActualWork.IsNotNull(() => _timer.Start());
    }
}