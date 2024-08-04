using UnityEngine;

namespace Units.Villagers.States
{
    public class MoveToResource : BaseStateVillager
    {
        public MoveToResource(Villager villager) : base(villager) { }

        public override void OnEnter()
        {
            villager.SetStateName("Move To Resource");
        }

        public override void OnUpdate()
        {
            ToResource(villager, villager.ActualWork.Position);
        }

        private static void ToResource(Villager villager, Vector3 position)
        {
            villager.SetDestination(position);
        }
    }
}