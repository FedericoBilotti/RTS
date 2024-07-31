using Manager;
using UnityEngine;

namespace Structures
{
    public class Center : MonoBehaviour, IStorage
    {
        public Vector3 Position => transform.position;

        private void OnEnable() => GameManager.Instance.AddCenter(this);
        private void OnDisable() => GameManager.Instance.RemoveCenter(this);

        public StorageTypes GetStorageType() => StorageTypes.Center;
    }
}
