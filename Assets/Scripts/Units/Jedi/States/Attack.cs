using Units.SO;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Units.Jedi.States
{
    public class Attack : BaseStateJedi
    {
        private readonly CountdownTimer _timer;

        public Attack(Jedi jedi, NavMeshAgent agent, JediSO jediSO) : base(jedi, agent, jediSO)
        {
            _timer = new CountdownTimer(0.5f);

            _timer.onTimerStop += AttackEnemy;
            _timer.onTimerStop += ResetTimer;
        }
        
        ~Attack()
        {
            _timer.onTimerStop -= AttackEnemy;
            _timer.onTimerStop -= ResetTimer;
        }

        public override void OnEnter()
        {
            // Reproducir animación de Atacar.
            jedi.SetStateName("Attack");
            
            ResetTimer();
        }

        public override void OnUpdate() => _timer.Tick(Time.deltaTime);

        private void AttackEnemy()
        {
            jedi.GetTarget().TakeDamage(jediSO.Damage);
        }

        private void ResetTimer()
        {
            _timer.Reset();
            _timer.Start();
        }
    }
}