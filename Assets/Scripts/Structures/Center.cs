using Manager;
using Player;
using Structures.Storages;
using UnityEngine;

namespace Structures
{
    public class Center : MonoBehaviour, IStorage
    {
        public Vector3 Position => transform.position;

        public ResourcesManager.ResourceType GetStorageType => ResourcesManager.ResourceType.All;
        
        public EFaction Faction { get; private set; }
        public void SetFaction(EFaction faction) => Faction = faction;

        private void OnEnable()
        {
            GameManager.Instance.AddCenter(this);
            GameManager.Instance.AddStorage(this);
        }

        private void OnDisable()
        {
            GameManager.Instance.RemoveCenter(this);
            GameManager.Instance.RemoveStorage(this);
        }
    }
}
