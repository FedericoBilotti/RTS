using Manager;
using UnityEngine;

namespace Structures.Storages
{
    public class GoldStorage : MonoBehaviour, IStorage
    {
        public Vector3 Position => transform.position;
        public ResourcesManager.ResourceType GetStorageType => ResourcesManager.ResourceType.Gold;

        private void OnEnable() => GameManager.Instance.AddStorage(this);
        private void OnDisable() => GameManager.Instance.RemoveStorage(this);
    }
}