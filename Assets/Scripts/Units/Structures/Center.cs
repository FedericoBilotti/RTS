using Manager;
using Units.Structures.Storages;

namespace Units.Structures
{
    public class Center : Storage
    {
        private void Start()
        {
            StorageType = ResourcesManager.ResourceType.All;
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
