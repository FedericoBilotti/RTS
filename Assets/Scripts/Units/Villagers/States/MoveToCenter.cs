using UnityEngine;

namespace Units.Villagers.States
{
    public class MoveToCenter : BaseStateVillager
    {
        private readonly Villager _villager;

        public MoveToCenter(Villager villager) : base(villager) {}

        public override void OnEnter()
        {
            ToCenter(villager, villager.GetStorage().Position);
            
            villager.SetStateName("Move To Center");
        }

        public override void OnUpdate()
        {
            if (!IsNearCenter()) return;
            
            villager.AddResourceToStorage();
        }

        public override void OnExit() => villager.SetStorage(null);

        private bool IsNearCenter() => Vector3.Distance(villager.transform.position, villager.GetStorage().Position) < 3f;
        private static void ToCenter(Villager villager, Vector3 position) => villager.SetDestination(position);
    }
}