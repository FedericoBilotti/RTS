using Manager;
using Units.Resources;
using UnityEngine;
using Utilities;

namespace Units.Villagers.States
{
    public class Mining : BaseState
    {
        private readonly Villager _villager;
        private readonly CountdownTimer _timer = new(2f);
        
        private Resource _resource;
        private ResourcesManager.ResourceType _resourceType;

        public Mining(Villager villager) => _villager = villager;

        public override void OnEnter()
        {
            _villager.StopMovement();
            
            _resource = _villager.GetResource();
            _resourceType = _resource.GetResourceType();

            _timer.Reset(_resource.GetTimeToGiveResource());
            _timer.onTimerStop += AddResource;
            _timer.onTimerStop += StartTimer;
            _timer.Start();

            _villager.SetStateName("Chop");
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

        private void AddResource() => _villager.AddResourceToInventory(_resourceType, _resource.ProvideResource());
        private void StartTimer() => _villager.GetResource().IsNotNull(() => _timer.Start());
    }
}