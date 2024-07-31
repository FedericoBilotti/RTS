using Manager;
using Structures.Storages;
using UnityEngine;

namespace Units.Villagers.States
{
    public class MoveToStorage : BaseStateVillager
    {
        public MoveToStorage(Villager villager) : base(villager) { }

        public override void OnEnter()
        {
            // If my storage is null, set it to the nearest center
            villager.SetStorage(villager.GetStorage() ?? GameManager.Instance.NearCenter(villager));

            Vector3 destination = villager.GetStorage().Position;
            MoveToNearStorage(villager, destination);

            villager.SetStateName("Move To Storage");
        }

        public override void OnUpdate()
        {
            IStorage actualStorage = villager.GetStorage();

            if (IsNearStorage(actualStorage)) return;

            AddResourceToStorage(actualStorage);
        }

        private bool IsNearStorage(IStorage actualStorage) => (actualStorage.Position - villager.transform.position).sqrMagnitude > 5f * 5f;

        private void AddResourceToStorage(IStorage actualStorage)
        {
            ResourcesManager.ResourceType storage = actualStorage.GetStorageType;

            if (storage == ResourcesManager.ResourceType.All)
            {
                villager.AddResourceToStorage();
            }
            else
            {
                // -> Automaticamente detecta el tipo de recurso que estaba recogiendo antes, ya que va al storage que recogia recursos
                ResourcesManager.ResourceType resourceType = villager.GetResourceType();
                villager.AddSpecificResourceToStorage(resourceType);
                Debug.Log("Se añadio un recurso específico");
            }
        }

        private static void MoveToNearStorage(Villager villager, Vector3 destination) => villager.SetDestination(destination);
    }
}