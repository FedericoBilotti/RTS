using Manager;
using Structures;
using Structures.Storages;
using Units.Resources;
using UnityEngine;
using Utilities;

namespace Units.Villagers.States
{
    public class Chop : BaseStateVillager
    {
        private readonly CountdownTimer _timer = new(2f);

        private Resource _resource;
        private ResourcesManager.ResourceType _resourceType;

        public Chop(Villager villager) : base(villager) { }

        public override void OnEnter()
        {
            villager.StopMovement();
            villager.SetStorage(GameManager.Instance.NearStorage(villager, ResourcesManager.ResourceType.Wood));
            
            _resource = villager.GetResource();
            _resourceType = _resource.GetResourceType();

            _timer.Reset(_resource.GetTimeToGiveResource());
            _timer.onTimerStop += AddResource;
            _timer.onTimerStop += StartTimer;
            _timer.Start();

            villager.SetStateName("Chop");
        }

        // Play chop animation
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

        private void AddResource() => villager.AddResourceToInventory(_resourceType, _resource.ProvideResource());
        private void StartTimer() => villager.GetResource().IsNotNull(() => _timer.Start());
    }
}