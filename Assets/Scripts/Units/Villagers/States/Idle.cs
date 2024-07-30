using Units.SO;
using UnityEngine.AI;

namespace Units.Villagers.States
{
    public class Idle : BaseState
    {
        private readonly Villager _villager;
        private readonly NavMeshAgent _agent;
        private readonly UnitSO _unitSO;

        public Idle(Villager villager, NavMeshAgent agent, UnitSO unitSo)
        {
            _villager = villager;
            _agent = agent;
            _unitSO = unitSo;
        }
        
        public override void OnEnter()
        {
            _villager.SetStateName("Idle");
            _agent.stoppingDistance = _unitSO.StoppingDistanceToAttack;
        }
        // Play Idle Animation
    }
}