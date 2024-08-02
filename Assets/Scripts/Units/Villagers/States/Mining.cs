using Manager;
using Units.Resources;
using UnityEngine;
using Utilities;

namespace Units.Villagers.States
{
    public class Mining : BaseStateVillager
    {
        private readonly CountdownTimer _timer = new(2f);

        //     private Resource _resource;
        //     private ResourcesManager.ResourceType _resourceType;
        //
        public Mining(Villager villager) : base(villager) { }
        //
        //     public override void OnEnter()
        //     {
        //         villager.StopMovement();
        //         villager.SetStorage(GameManager.Instance.NearStorage(villager, ResourcesManager.ResourceType.Gold));
        //
        //         _resource = villager.ActualResource;
        //         _resourceType = _resource.GetResourceSO().ResourceType;
        //
        //         _timer.Reset(_resource.GetResourceSO().TimeToGiveResource);
        //         _timer.onTimerStop += AddResource;
        //         _timer.onTimerStop += StartTimer;
        //         _timer.Start();
        //
        //         villager.SetStateName("Mining");
        //     }
        //
        //     // Play mining animation
        //     public override void OnUpdate()
        //     {
        //         _timer.Tick(Time.deltaTime);
        //     }
        //
        //     public override void OnExit()
        //     {
        //         _timer.onTimerStop -= AddResource;
        //         _timer.onTimerStop -= StartTimer;
        //         _timer.Stop();
        //     }
        //
        //     private void AddResource() => villager.AddResourceToInventory(_resourceType, _resource.ProvideResource());
        //     private void StartTimer() => villager.ActualResource.IsNotNull(() => _timer.Start());
    }
}