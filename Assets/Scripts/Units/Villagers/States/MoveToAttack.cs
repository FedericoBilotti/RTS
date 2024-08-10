using Units.SO;
using UnityEngine;
using UnityEngine.AI;

namespace Units.Villagers.States
{
    public class MoveToAttack : BaseStateVillager
    {
        private readonly NavMeshAgent _agent;
        private readonly VillagerSO _villagerSO;

        public MoveToAttack(Villager villager, NavMeshAgent agent, VillagerSO villagerSO) : base(villager)
        {
            _agent = agent;
            _villagerSO = villagerSO;
        }

        public override void OnEnter()
        {
            _agent.stoppingDistance = _villagerSO.StoppingDistanceToAttack;
            
            villager.SetStateName("MoveToAttack");
        }

        public override void OnUpdate()
        {
            Vector3 destination = villager.GetTarget().GetPosition();
            
            villager.SetDestination(destination);
        }
    }
}