using Units.Resources;
using UnityEngine;

namespace Player.OrderActions
{
    /// <summary>
    /// Mueve a las unidades en formación -> Siempre debe permanece como última opción en la lista.
    /// </summary>
    public class MoveUnitsInFormation : IOrderStrategy
    {
        public bool Execute(UnitManager unitManager, RaycastHit hit)
        {
            Vector3 desiredPosition = hit.point;
            MoveUnits(unitManager, desiredPosition);
            AssignWorkToVillagers(unitManager, null);
            return true;
        }

        private static void MoveUnits(UnitManager unitManager, Vector3 destination) => unitManager.MoveUnitsInFormation(destination);
        private static void AssignWorkToVillagers(UnitManager unitManager, IWork work) => unitManager.AssignWorkToUnits(work);
    }
}