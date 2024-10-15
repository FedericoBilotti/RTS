using Manager;
using Units.Structures.Storages;
using UnityEngine;

namespace Units.Villagers.States
{
    public class MoveToStorage : BaseStateVillager
    {
        public MoveToStorage(Villager villager) : base(villager) { }

        public override void OnEnter()
        {
            IStorage storage = villager.Storage;

            villager.SetStorage(storage);
            villager.SetDestination(storage.Position);
            villager.SetStateName("Move To Storage");
        }

        public override void OnUpdate()
        {
            IStorage actualStorage = villager.Storage;

            if (!IsNearStorage(actualStorage)) return;

            AddResourceToStorage(actualStorage);
            villager.StopMovement();
        }

        public override void OnExit()
        {
            villager.SetStorage(null);
        }

        private bool IsNearStorage(IStorage actualStorage) => (actualStorage.Position - villager.transform.position).sqrMagnitude < 5f * 5f;

        private void AddResourceToStorage(IStorage actualStorage)
        {
            ResourcesManager.ResourceType resourceStorageType = actualStorage.StorageType;

            if (AddResourcesToCenter(resourceStorageType)) return;

            AddResource(resourceStorageType);
        }

        private bool AddResourcesToCenter(ResourcesManager.ResourceType resourceStorageType)
        {
            if (resourceStorageType != ResourcesManager.ResourceType.All) return false;

            villager.AddResourcesToCenter();
            return true;
        }

        private void AddResource(ResourcesManager.ResourceType resourceStorageType)
        {
            if (!villager.HasResourceInInventory(resourceStorageType))
            {
                Debug.Log($"El tipo de recurso que se quiere añadir no se tiene actualmente en el inventario: {resourceStorageType}");
                villager.SetStorage(null);
                return;
            }

            villager.AddResourceToStorage(resourceStorageType);
        }
    }
}