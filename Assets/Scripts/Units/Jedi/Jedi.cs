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
            var attack = new Attack(this, agent, _jediSO);
            var searchNearEnemy = new SearchNearEnemy(this, agent, _jediSO);
            
            IdleTransitions(idle, moving, attack);
            MovingTransitions(moving, idle, attack);
            AttackTransition(searchNearEnemy, attack);
            
            fsm.SetState(idle);
        }

        private void IdleTransitions(Idle idle, Moving moving, Attack attack)
        {
            fsm.AddTransition(idle, moving, new FuncPredicate(() => agent.hasPath));
            fsm.AddTransition(idle, attack, new FuncPredicate(() => targetable != null && !targetable.IsDead()));
        }

        private void MovingTransitions(Moving moving, Idle idle, Attack attack)
        {
            fsm.AddTransition(moving, idle, new FuncPredicate(() => !agent.hasPath));
            fsm.AddTransition(moving, attack, new FuncPredicate(() => targetable != null && !targetable.IsDead()));
        }

        private void AttackTransition(SearchNearEnemy searchNearEnemy, Attack attack)
        {
            fsm.AddTransition(attack, searchNearEnemy, new FuncPredicate(() => targetable == null));
        }

        private void SearchNearEnemy(SearchNearEnemy searchNearEnemy, Attack attack, Idle idle)
        {
            fsm.AddTransition(searchNearEnemy, attack, new FuncPredicate(() => targetable != null && !targetable.IsDead()));
            fsm.AddTransition(searchNearEnemy, idle, new FuncPredicate(() => targetable == null));
        }
    }
}