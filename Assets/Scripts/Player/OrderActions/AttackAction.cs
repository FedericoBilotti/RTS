using UnityEngine;

namespace Player.OrderActions
{
    /// <summary>
    /// Genera la acción de atacar a algo que sea dañable
    /// </summary>
    public class AttackAction : IOrderStrategy
    {
        public bool Execute(UnitManager unitManager, RaycastHit hit)
        {
            if (!hit.transform.TryGetComponent(out IDamageable damageable)) return false;
            if (damageable.GetFaction() == unitManager.Faction) return false;
            
            SetEnemyTargets(unitManager, damageable);
            return true;
        }
        
        private static void SetEnemyTargets(UnitManager unitManager, IDamageable damageable) => unitManager.SetEnemyTargets(damageable);
    }
}