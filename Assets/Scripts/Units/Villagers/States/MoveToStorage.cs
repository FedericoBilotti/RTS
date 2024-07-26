
using Manager;
using Structures;
using UnityEngine;

namespace Units.Villagers.States
{
    public class MoveToStorage : BaseState
    {
        private Center _actualCenter;
        private readonly Villager _villager;

        public MoveToStorage(Villager villager) => _villager = villager;

        public override void OnEnter()
        {
            _actualCenter = GameManager.Instance.NearCenter(_villager);
        }

        public override void OnFixedUpdate()
        {
            Vector3 destination = _actualCenter.transform.position;
            Vector3 distance = destination - _villager.transform.position;
            
            MoveToNearStorage(destination);

            if (distance.magnitude > 5f) return;
            
            ResourcesManager.Instance.AddResource(ResourcesManager.ResourceType.Gold, _villager.GetResource().GetResourceAmountToGive());
            _villager.SetAmountResource(0);
        }

        private void MoveToNearStorage(Vector3 destination)
        {
            
            _villager.SetDestination(destination);
        }
    }
}