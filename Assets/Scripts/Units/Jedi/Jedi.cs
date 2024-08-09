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

        public void SetStateName(string state) => _actualState = state; // Debug.

        private void CreateFSM()
        {
            fsm = new FiniteStateMachine();

            var idle = new Idle(this, agent, _jediSO);
            var moving = new Moving(this, agent, _jediSO);
            var moveToAttack = new MoveToAttack(this, agent, _jediSO);
            var attack = new Attack(this, agent, _jediSO);
            var searchNearEnemy = new SearchNearEnemy(this, agent, _jediSO);

            IdleTransitions(idle, moving, moveToAttack);
            MovingTransitions(moving, idle, moveToAttack);
            MoveToAttack(moveToAttack, searchNearEnemy, attack);
            AttackTransition(searchNearEnemy, attack, moveToAttack);
            SearchNearEnemy(searchNearEnemy, moveToAttack, idle);

            fsm.SetState(idle);
        }

        private void IdleTransitions(Idle idle, Moving moving, MoveToAttack moveToAttack)
        {
            fsm.AddTransition(idle, moving, new FuncPredicate(() => targetable == null && agent.hasPath));
            fsm.AddTransition(idle, moveToAttack, new FuncPredicate(() => targetable != null && !targetable.IsDead()));
        }

        private void MovingTransitions(Moving moving, Idle idle, MoveToAttack moveToAttack)
        {
            fsm.AddTransition(moving, idle, new FuncPredicate(() => !agent.hasPath));
            fsm.AddTransition(moving, moveToAttack, new FuncPredicate(() => targetable != null && !targetable.IsDead()));
        }

        private void MoveToAttack(MoveToAttack moveToAttack, SearchNearEnemy searchNearEnemy, Attack attack)
        {
            fsm.AddTransition(moveToAttack, attack, new FuncPredicate(CanAttack));
            fsm.AddTransition(moveToAttack, searchNearEnemy, new FuncPredicate(() => targetable.IsDead()));
        }

        private void AttackTransition(SearchNearEnemy searchNearEnemy, Attack attack, MoveToAttack moveToAttack)
        {
            fsm.AddTransition(attack, moveToAttack, new FuncPredicate(() => !CanAttack()));
            fsm.AddTransition(attack, searchNearEnemy, new FuncPredicate(() => targetable.IsDead()));
        }

        private void SearchNearEnemy(SearchNearEnemy searchNearEnemy, MoveToAttack moveToAttack, Idle idle)
        {
            fsm.AddTransition(searchNearEnemy, idle, new FuncPredicate(() => targetable == null));
            fsm.AddTransition(searchNearEnemy, moveToAttack, new FuncPredicate(() => targetable != null));
        }

        private bool CanAttack()
        {
            return !targetable.IsDead() && Vector3.Distance(transform.position, targetable.GetPosition()) < agent.stoppingDistance;
        }
    }
}