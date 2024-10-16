using Units;
using UnityEngine;

namespace Player.OrderActions
{
    /// <summary>
    /// Genera la acción de atacar a algo que sea dañable
    /// </summary>
    public class AttackAction : IOrderStrategy
    {
        public bool Execute(PlayerManager playerManager, RaycastHit hit)
        {
            if (!hit.transform.TryGetComponent(out ITargetable targetable)) return false;
            if (targetable.IsDead()) return false;
            if (targetable.GetFaction() == playerManager.Faction) return false;
            
            SetEnemyTargets(playerManager, targetable);
            return true;
        }
        
        private static void SetEnemyTargets(PlayerManager playerManager, ITargetable targetable) => playerManager.SetEnemyTargets(targetable);
    }
}