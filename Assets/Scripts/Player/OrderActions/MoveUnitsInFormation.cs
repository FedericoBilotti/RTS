using Units.Resources;
using UnityEngine;

namespace Player.OrderActions
{
    /// <summary>
    /// Mueve a las unidades en formación. -> Siempre debe permanece como última opción en la lista.
    /// </summary>
    public class MoveUnitsInFormation : IOrderStrategy
    {
        public bool Execute(PlayerManager playerManager, RaycastHit hit)
        {
            Vector3 desiredPosition = hit.point;
            MoveUnits(playerManager, desiredPosition);
            AssignWorkToVillagers(playerManager, null);
            return true;
        }

        private static void MoveUnits(PlayerManager playerManager, Vector3 destination) => playerManager.MoveUnitsInFormation(destination);
        private static void AssignWorkToVillagers(PlayerManager playerManager, IWork work) => playerManager.AssignWorkToUnits(work);
    }
}