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
            villager.SetStorage(villager.ActualStorage ?? GameManager.Instance.NearCenter(villager));

            Vector3 destination = villager.ActualStorage.Position;
            MoveToNearStorage(villager, destination);

            villager.SetStateName("Move To Storage");
        }

        public override void OnUpdate()
        {
            IStorage actualStorage = villager.ActualStorage;

            if (IsNearStorage(actualStorage)) return;

            AddResourceToStorage(actualStorage);
            villager.StopMovement();
        }

        public override void OnExit()
        {
            villager.SetStorage(null);
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
                ResourcesManager.ResourceType resourceType = villager.PreviousWork.GetResourceSO().ResourceType;
                
                villager.AddSpecificResourceToStorage(resourceType);
                Debug.Log($"Se añadio: {resourceType} al storage");
            }
        }

        private static void MoveToNearStorage(Villager villager, Vector3 destination) => villager.SetDestination(destination);
    }
}