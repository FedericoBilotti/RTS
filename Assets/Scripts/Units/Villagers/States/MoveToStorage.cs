using System;
using Manager;
using Structures;
using TMPro.EditorUtilities;
using Units.Resources;
using UnityEngine;

namespace Units.Villagers.States
{
    public class MoveToStorage : BaseStateVillager
    {
        public MoveToStorage(Villager villager) : base(villager) { }

        public override void OnEnter()
        {
            villager.SetStorage(villager.GetStorage() ?? GameManager.Instance.NearCenter(villager));

            Vector3 destination = villager.GetStorage().Position;
            MoveToNearStorage(destination);

            villager.SetStateName("Move To Storage");
        }

        public override void OnUpdate()
        {
            IStorage actualStorage = villager.GetStorage();
            Vector3 distance = actualStorage.Position - villager.transform.position;

            Debug.Log("hola");
            if (distance.magnitude > 5f) return;

            StorageTypes storage = actualStorage.GetStorageType();

            if (storage == StorageTypes.Center)
            {
                villager.AddResourceToStorage();
            }
            else
            {
                // -> Automaticamente detecta el tipo de recurso que estaba recogiendo antes, ya que va al storage que recogia recursos
                var resourceType = villager.GetResourceType();
                villager.AddSpecificResourceToStorage(resourceType);
            } 
            
            villager.SetStorage(null);
            Debug.Log("Añadi el recurso al storage");
            
            // else if (storage == StorageTypes.Food) villager.AddSpecificResourceToStorage(ResourcesManager.ResourceType.Food);
            // else if (storage == StorageTypes.Wood) villager.AddSpecificResourceToStorage(ResourcesManager.ResourceType.Wood);
            // else if (storage == StorageTypes.Gold) villager.AddSpecificResourceToStorage(ResourcesManager.ResourceType.Gold);
        }

        private void MoveToNearStorage(Vector3 destination)
        {
            villager.SetDestination(destination);
        }
    }
}