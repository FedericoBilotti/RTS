using Manager;
using Player;
using Units.SO;
using UnityEngine;
using UnityEngine.AI;

namespace Units.Villagers.States
{
    public class MoveToResource : BaseStateVillager
    {
        private readonly NavMeshAgent _agent;
        private readonly VillagerSO _villagerSO;

        public MoveToResource(Villager villager, NavMeshAgent agent, VillagerSO villagerSO) : base(villager)
        {
            _agent = agent;
            _villagerSO = villagerSO;
        }

        public override void OnEnter()
        {
            _agent.stoppingDistance = _villagerSO.StoppingDistanceToWork;
            AddToWorkingVillagerList(villager);
            SetObstacleAvoidance(_agent, ObstacleAvoidanceType.NoObstacleAvoidance);

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

        private static void AddToWorkingVillagerList(Villager villager)
        {
            ResourcesManager.ResourceType resourceType = villager.ActualWork.GetResourceSO().ResourceType;
            UnitManager.Instance.AddWorkingVillager(villager, resourceType);
        }

        private static void SetObstacleAvoidance(NavMeshAgent agent, ObstacleAvoidanceType type)
        {
            agent.obstacleAvoidanceType = type;
        }
    }
}