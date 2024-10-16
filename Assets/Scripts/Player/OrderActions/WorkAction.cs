using Units.Resources;
using UnityEngine;

namespace Player.OrderActions
{
    /// <summary>
    /// Genera la acción de poner a trabajar a los villagers.
    /// </summary>
    public class WorkAction : IOrderStrategy
    {
        public bool Execute(PlayerManager playerManager, RaycastHit hit)
        {
            if (!hit.transform.TryGetComponent(out IWork work)) return false;
            
            AssignWorkToUnits(playerManager, work);
            return true;
        }

        private static void AssignWorkToUnits(PlayerManager playerManager, IWork work) => playerManager.AssignWorkToUnits(work);
    }
}