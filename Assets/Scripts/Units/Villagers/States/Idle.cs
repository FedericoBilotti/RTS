using Units.SO;
using UnityEngine.AI;

namespace Units.Villagers.States
{
    public class Idle : BaseStateVillager
    {
        private readonly NavMeshAgent _agent;
        private readonly UnitSO _unitSO;

        public Idle(Villager villager, NavMeshAgent agent, UnitSO unitSo) : base(villager)
        {
            _agent = agent;
            _unitSO = unitSo;
        }
        
        public override void OnEnter()
        {
            villager.SetStateName("Idle");
            _agent.stoppingDistance = _unitSO.StoppingDistanceToIdle;
            villager.StopMovement();
        }
        // Play Idle Animation
    }
}