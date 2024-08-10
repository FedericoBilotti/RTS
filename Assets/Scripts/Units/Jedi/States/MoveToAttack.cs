using Units.SO;
using UnityEngine;
using UnityEngine.AI;

namespace Units.Jedi.States
{
    public class MoveToAttack : BaseStateJedi
    {
        public MoveToAttack(Jedi jedi, NavMeshAgent agent, JediSO jediSO) : base(jedi, agent, jediSO) { }

        public override void OnEnter()
        {
            agent.stoppingDistance = jediSO.StoppingDistanceToAttack;
            
            jedi.SetStateName("MoveToAttack");
        }

        public override void OnUpdate()
        {
            Vector3 destination = jedi.GetTarget().GetPosition();
            
            jedi.SetDestination(destination);
        }
    }
}