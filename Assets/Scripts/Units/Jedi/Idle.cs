using Units.SO;
using UnityEngine.AI;

namespace Units.Jedi
{
    public class Idle : BaseStateJedi
    {
        public Idle(Jedi jedi, NavMeshAgent agent, JediSO jediSO) : base(jedi, agent, jediSO) { }

        public override void OnEnter()
        {
            agent.stoppingDistance = jediSO.StoppingDistanceToIdle;
            
            // Reproducir animación de Idle.
            jedi.SetStateName("Idle");
        }
    }
}