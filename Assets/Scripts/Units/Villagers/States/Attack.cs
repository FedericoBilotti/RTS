using Units.SO;
using UnityEngine;
using Utilities;

namespace Units.Villagers.States
{
    public class Attack : BaseStateVillager
    {
        private readonly VillagerSO _villagerSO;
        private readonly CountdownTimer _timer;
        
        public Attack(Villager villager, VillagerSO villagerSO) : base(villager)
        {
            _villagerSO = villagerSO;
            
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
            villager.SetStateName("Attack");
            
            ResetTimer();
        }

        public override void OnUpdate() => _timer.Tick(Time.deltaTime);

        private void AttackEnemy()
        {
            villager.GetTarget().TakeDamage(_villagerSO.Damage);
        }

        private void ResetTimer()
        {
            _timer.Reset();
            _timer.Start();
        }
    }
}