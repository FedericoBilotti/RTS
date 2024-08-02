using Unity.VisualScripting;
using UnityEngine;

namespace Units.Villagers.States
{
    public class MoveToResource : BaseStateVillager
    {
        public MoveToResource(Villager villager) : base(villager) { }

        public override void OnEnter()
        {
            ToResource(villager, villager.ActualWork.Position);

            villager.SetStateName("Move To Resource");
        }

        private static void ToResource(Villager villager, Vector3 position)
        {
            villager.SetDestination(position);
        }
    }
}