using Manager;
using Structures.Storages;

namespace Structures
{
    public class Center : Storage
    {
        private void Start()
        {
            GetStorageType = ResourcesManager.ResourceType.All;
        }
        
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
