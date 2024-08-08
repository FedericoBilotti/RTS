using System;

namespace Units
{
    public abstract class EntityLife
    {
        protected readonly EntityLifeSO entityEntityLifeSO;
        protected int actualLife;

        // Eventos
        public Action<float> onTakeDamage = delegate { };
        public Action onDeadUnit = delegate { };

        protected EntityLife(EntityLifeSO entityEntityLifeSO)
        {
            this.entityEntityLifeSO = entityEntityLifeSO;

            actualLife = entityEntityLifeSO.MaxLife;
        }

        public abstract void TakeDamage(int damage);
        protected abstract void Dead();
    }
}