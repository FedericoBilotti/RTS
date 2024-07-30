using UnityEngine;

namespace Units.Villagers.States
{
    public class MoveToCenter : BaseState
    {
        private readonly Villager _villager;

        public MoveToCenter(Villager villager) => _villager = villager;

        public override void OnEnter()
        {
            ToCenter(_villager, _villager.GetCenter().transform);
            
            _villager.SetStateName("Move To Center");
        }

        public override void OnExit() => _villager.SetCenter(null);

        private static void ToCenter(Villager villager, Transform resourceTransform)
        {
            villager.SetDestination(resourceTransform.position);
        }
    }
}