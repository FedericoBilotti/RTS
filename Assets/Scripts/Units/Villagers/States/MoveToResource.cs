using UnityEngine;
using UnityEngine.AI;

namespace Units.Villagers.States
{
    public class MoveToResource : BaseStateVillager
    {
        private readonly NavMeshAgent _agent;
        public MoveToResource(Villager villager, NavMeshAgent agent) : base(villager)
        {
            _agent = agent;
        }

        public override void OnEnter()
        {
            villager.SetStateName("Move To Resource");
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
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