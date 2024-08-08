using Units;
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
            if (!hit.transform.TryGetComponent(out ITargetable targetable)) return false;
            if (targetable.GetFaction() == unitManager.Faction) return false;
            
            SetEnemyTargets(unitManager, targetable);
            return true;
        }
        
        private static void SetEnemyTargets(UnitManager unitManager, ITargetable targetable) => unitManager.SetEnemyTargets(targetable);
    }
}