using System;
using UnityEngine;

namespace Units
{
    public abstract class EntityLife : MonoBehaviour, IDamageable
    {
        [SerializeField] protected EntityLifeSO entityLifeSO;
        protected int actualLife;

        // Eventos
        public event Action<float> OnTakeDamage = delegate { };
        public event Action OnDeadUnit = delegate { };

        private void Awake() => actualLife = entityLifeSO.MaxLife;

        private void Start()
        {
            OnDeadUnit += () => gameObject.SetActive(false); // Regresar a una pool futura.
        }

        public virtual void TakeDamage(int damage)
        {
            actualLife = Mathf.Max(actualLife -= damage, 0);

            OnTakeDamage.Invoke(entityLifeSO.CalculateLifePercentage(actualLife));

            Dead();
        }

        protected virtual void Dead()
        {
            if (!IsDead()) return;

            OnDeadUnit.Invoke();
        }        
        
        public bool IsDead() => actualLife <= 0;
    }
}