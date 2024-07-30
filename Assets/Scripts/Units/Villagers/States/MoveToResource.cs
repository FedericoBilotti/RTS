using UnityEngine;

namespace Units.Villagers.States
{
    public class MoveToResource : BaseState
    {
        private readonly Villager _villager;

        public MoveToResource(Villager villager) => _villager = villager;

        public override void OnEnter()
        {
            ToResource(_villager, _villager.GetResource().transform);
            
            _villager.SetStateName("Move To Resource");
        }

        private static void ToResource(Villager villager, Transform resourceTransform)
        {
            villager.SetDestination(resourceTransform.position);
        }
    }
}