using StateMachine;
using Units.SO;
using UnityEngine.AI;

namespace Units.Jedi.States
{
    public abstract class BaseStateJedi : IState
    {
        protected readonly Jedi jedi;
        protected  readonly NavMeshAgent agent;
        protected  readonly JediSO jediSO;

        protected BaseStateJedi(Jedi jedi, NavMeshAgent agent, JediSO jediSO)
        {
            this.jedi = jedi;
            this.agent = agent;
            this.jediSO = jediSO;
        }
        
        public virtual void OnEnter()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnFixedUpdate()
        {
            
        }

        public virtual void OnExit()
        {
            
        }
    }
}