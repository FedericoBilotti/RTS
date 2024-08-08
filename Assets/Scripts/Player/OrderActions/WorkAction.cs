using Units.Resources;
using UnityEngine;

namespace Player.OrderActions
{
    /// <summary>
    /// Genera la acción de poner a trabajar a los villagers.
    /// </summary>
    public class WorkAction : IOrderStrategy
    {
        public bool Execute(UnitManager unitManager, RaycastHit hit)
        {
            if (!hit.transform.TryGetComponent(out IWork work)) return false;
            
            AssignWorkToUnits(unitManager, work);
            return true;
        }

        private static void AssignWorkToUnits(UnitManager unitManager, IWork work) => unitManager.AssignWorkToUnits(work);
    }
}