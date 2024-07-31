using UnityEngine;

namespace Units.Villagers.States
{
    public class MoveToResource : BaseStateVillager
    {
        public MoveToResource(Villager villager) : base(villager) { }

        public override void OnEnter()
        {
            ToResource(villager, villager.GetResource().transform);

            villager.SetStateName("Move To Resource");
        }

        private static void ToResource(Villager villager, Transform resourceTransform)
        {
            villager.SetDestination(resourceTransform.position);
        }
    }
}