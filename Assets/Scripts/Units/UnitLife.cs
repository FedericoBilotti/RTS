using UnityEngine;

namespace Units
{
    public class UnitLife : EntityLife
    {
        public UnitLife(EntityLifeSO entityEntityLifeSO) : base(entityEntityLifeSO) { }
        
        public override void TakeDamage(int damage)
        {
            actualLife = Mathf.Max(actualLife -= damage, 0);
            
            onTakeDamage.Invoke(damage);
            
            Dead();
        }

        protected override void Dead()
        {
            if (actualLife > 0) return;
            
            onDeadUnit.Invoke();
        }
    }
}
