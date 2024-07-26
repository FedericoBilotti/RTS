using UnityEngine;

namespace Units.Villagers.States
{
    public class MoveToResource : BaseState
    {
        private readonly Villager _villager;

        public MoveToResource(Villager villager)
        {
            _villager = villager;
        }

        public override void OnFixedUpdate()
        {
            MoveToActualResource();
        }

        private void MoveToActualResource()
        {
            Vector3 destination = _villager.GetResource().transform.position;
            
            _villager.SetDestination(destination);
        }
    }
}