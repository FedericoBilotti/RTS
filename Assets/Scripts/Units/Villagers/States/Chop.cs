using Manager;
using Units.Resources;
using UnityEngine;
using Utilities;

namespace Units.Villagers.States
{
    public class Chop : BaseState
    {
        private readonly Villager _villager;
        private readonly CountdownTimer _timer = new(2f);

        private Resource _resource;
        private ResourcesManager.ResourceType _resourceType;

        public Chop(Villager villager) => _villager = villager;

        public override void OnEnter()
        {
            _villager.StopMovement();
            
            _resource = _villager.GetResource();
            _resourceType = _resource.GetResourceType();

            _timer.Reset(_resource.GetTimeToGiveResource());
            _timer.onTimerStop += AddResource;
            _timer.onTimerStop += SetResource;
            _timer.onTimerStop += StartTimer;
            _timer.Start();

            _villager.SetName("Chop");
        }

        // Play chop animation
        public override void OnUpdate()
        {
            _timer.Tick(Time.deltaTime);
        }

        public override void OnExit()
        {
            _timer.onTimerStop -= AddResource;
            _timer.onTimerStop -= SetResource;
            _timer.onTimerStop -= StartTimer;
            _timer.Stop();
        }

        private void SetResource() => _villager.SetResource(_resource.GetActualAmount() <= 0 ? null : _resource);
        private void AddResource() => _villager.AddResourceToInventory(_resourceType, _resource.ProvideResource());
        
        private void StartTimer()
        {
            if (_villager.GetResource() == null) return;

            _timer.Start();
        }
    }
}