using System;
using UnityEngine;

namespace Units
{
    public abstract class EntityLife : MonoBehaviour, IDamageable
    {
        [SerializeField] protected EntityLifeSO entityLifeSO;
        protected int actualLife;

        // Eventos
        public Action<float> onTakeDamage = delegate { };
        public Action onDeadUnit = delegate { };

        private void Awake() => actualLife = entityLifeSO.MaxLife;

        public virtual void TakeDamage(int damage)
        {
            actualLife = Mathf.Max(actualLife -= damage, 0);

            onTakeDamage.Invoke(entityLifeSO.CalculateLifePercentage(actualLife));

            Dead();
        }

        protected virtual void Dead()
        {
            if (!IsDead()) return;

            onDeadUnit.Invoke();
        }        
        
        public bool IsDead() => actualLife <= 0;
    }
}