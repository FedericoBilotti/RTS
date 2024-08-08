using StateMachine;
using Units.SO;
using UnityEngine;

namespace Units.Jedi
{
    public class Jedi : Unit
    {
        [SerializeField] private string _actualState; // Para saber el estado en que me encuentro -> borrar después.
        [SerializeField] private JediSO _jediSO;


        private void Start()
        {
            CreateFSM();
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();

        public void SetStateName(string state) => _actualState = state; // Debug.

        private void CreateFSM()
        {
            fsm = new FiniteStateMachine();

            var idle = new Idle(this, agent, _jediSO);
            var moving = new Moving(this, agent, _jediSO);
            
            IdleTransitions(idle, moving);
            MovingTransitions(moving, idle);
            
            fsm.SetState(idle);
        }

        private void IdleTransitions(Idle idle, Moving moving)
        {
            fsm.AddTransition(idle, moving, new FuncPredicate(() => agent.hasPath));
        }

        private void MovingTransitions(Moving moving, Idle idle)
        {
            fsm.AddTransition(moving, idle, new FuncPredicate(() => !agent.hasPath));
        }
    }
}