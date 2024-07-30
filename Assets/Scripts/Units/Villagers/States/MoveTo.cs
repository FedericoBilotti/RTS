using Units.SO;
using UnityEngine.AI;

namespace Units.Villagers.States
{
    public class MoveTo : BaseState
    {
        private readonly Villager _villager;
        private readonly NavMeshAgent _agent;
        private readonly UnitSO _unitSO;

        public MoveTo(Villager villager, NavMeshAgent agent, UnitSO villagerSo)
        {
            _villager = villager;
            _agent = agent;
            _unitSO = villagerSo;
        }

        public override void OnEnter()
        {
            _villager.SetStateName("Move To");
            _agent.stoppingDistance = _unitSO.StoppingDistanceToIdle;
        }
    }
}