using Manager;
using UnityEngine;

namespace Structures.Storages
{
    public class WoodStorage : MonoBehaviour, IStorage
    {
        public Vector3 Position => transform.position;
        public ResourcesManager.ResourceType GetStorageType => ResourcesManager.ResourceType.Wood;

        private void OnEnable() => GameManager.Instance.AddStorage(this);
        private void OnDisable() => GameManager.Instance.RemoveStorage(this);
    }
}