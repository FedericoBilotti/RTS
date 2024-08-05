using Manager;
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

        private ResourcesManager.ResourceType _resourceType;

        public WorkVillager(Villager villager) : base(villager) { }

        public override void OnEnter()
        {
            _work = villager.ActualWork;
            _resourceType = _work.GetResourceSO().ResourceType;

            villager.SetStorage(GameManager.Instance.NearStorage(villager, _resourceType));
            villager.SetPreviousWork(villager.ActualWork);
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
            _timer.onTimerStop -= AddResource;
            _timer.onTimerStop -= StartTimer;
            _timer.Stop();
        }

        private void AddResource() => villager.AddResourceToInventory(_resourceType, _work.ProvideResource());
        private void StartTimer() => villager.ActualWork.IsNotNull(() => _timer.Start());
    }
}