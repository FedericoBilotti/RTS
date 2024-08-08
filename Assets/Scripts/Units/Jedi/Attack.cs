using Units.SO;
using UnityEngine.AI;

namespace Units.Jedi
{
    public class Attack : BaseStateJedi
    {
        public Attack(Jedi jedi, NavMeshAgent agent, JediSO jediSO) : base(jedi, agent, jediSO) { }

        public override void OnEnter()
        {
            agent.stoppingDistance = jediSO.StoppingDistanceToAttack;
            
            // Reproducir animación de Atacar.
            jedi.SetStateName("Attack");
        }
    }
}