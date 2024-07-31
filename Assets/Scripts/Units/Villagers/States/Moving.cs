using Units.SO;
using UnityEngine.AI;

namespace Units.Villagers.States
{
    public class Moving : BaseStateVillager
    {
        private readonly Villager _villager;
        private readonly NavMeshAgent _agent;
        private readonly UnitSO _unitSO;

        public Moving(Villager villager, NavMeshAgent agent, UnitSO villagerSo) : base(villager) 
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