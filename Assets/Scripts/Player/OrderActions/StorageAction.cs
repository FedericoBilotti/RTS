using Units.Structures.Storages;
using UnityEngine;

namespace Player.OrderActions
{
    /// <summary>
    /// Genera la acción de asignar el storage a los villagers
    /// </summary>
    public class StorageAction : IOrderStrategy
    {
        public bool Execute(UnitManager unitManager, RaycastHit hit)
        {
            if (!hit.transform.TryGetComponent(out IStorage storage)) return false;
            if (storage.GetFaction() != unitManager.Faction) return false;

            AssignStorageToVillagers(unitManager, storage);
            return true;
        }

        private static void AssignStorageToVillagers(UnitManager unitManager, IStorage storage) => unitManager.SetStorage(storage);
    }
}