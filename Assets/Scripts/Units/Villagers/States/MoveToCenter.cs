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

        public override void OnUpdate()
        {
            if (!IsNearCenter()) return;
            
            _villager.AddResourceToStorage();
        }

        public override void OnExit() => _villager.SetCenter(null);

        private bool IsNearCenter() => Vector3.Distance(_villager.transform.position, _villager.GetCenter().transform.position) < 3f;
        
        private static void ToCenter(Villager villager, Transform resourceTransform)
        {
            villager.SetDestination(resourceTransform.position);
        }
    }
}