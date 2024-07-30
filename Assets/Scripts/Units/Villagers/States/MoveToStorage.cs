using Manager;
using Structures;
using UnityEngine;

namespace Units.Villagers.States
{
    public class MoveToStorage : BaseState
    {
        private Center _actualCenter; // -> IStorage 
        private readonly Villager _villager;

        public MoveToStorage(Villager villager) => _villager = villager;

        public override void OnEnter()
        {   
            // En vez de GetCenter, seria -> GetStorage
            _actualCenter = _villager.GetCenter() ?? GameManager.Instance.NearCenter(_villager);

            _villager.SetStateName("MoveToStorage");
        }

        public override void OnFixedUpdate()
        {
            Vector3 destination = _actualCenter.transform.position;
            Vector3 distance = destination - _villager.transform.position;

            MoveToNearStorage(destination);

            if (distance.magnitude > 5f) return;

            _villager.AddResourceToStorage();
        }

        private void MoveToNearStorage(Vector3 destination)
        {
            _villager.SetDestination(destination);
        }
    }
}