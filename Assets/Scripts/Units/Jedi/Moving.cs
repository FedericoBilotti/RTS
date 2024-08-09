using Units.SO;
using UnityEngine.AI;

namespace Units.Jedi
{
    public class Moving : BaseStateJedi
    {
        public Moving(Jedi jedi, NavMeshAgent agent, JediSO jediSO) : base(jedi, agent, jediSO) { }

        public override void OnEnter()
        {
            // Reproducir animación de Caminar.
            jedi.SetStateName("Moving");
        }

        public override void OnUpdate()
        {
        }
    }
}