using Units.Structures.Storages;
using UnityEngine;

namespace Player.OrderActions
{
    /// <summary>
    /// Genera la acción de asignar el storage a los villagers
    /// </summary>
    public class StorageAction : IOrderStrategy
    {
        public bool Execute(PlayerManager playerManager, RaycastHit hit)
        {
            if (!hit.transform.TryGetComponent(out IStorage storage)) return false;
            if (storage.GetFaction() != playerManager.Faction) return false;

            AssignStorageToVillagers(playerManager, storage);
            return true;
        }

        private static void AssignStorageToVillagers(PlayerManager playerManager, IStorage storage) => playerManager.SetStorage(storage);
    }
}