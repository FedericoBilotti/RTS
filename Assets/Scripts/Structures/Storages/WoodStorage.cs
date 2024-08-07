using Manager;

namespace Structures.Storages
{
    public class WoodStorage : Storage
    {
        private void Start()
        {
            GetStorageType = ResourcesManager.ResourceType.Wood;
        }

        private void OnEnable() => GameManager.Instance.AddStorage(this);
        private void OnDisable() => GameManager.Instance.RemoveStorage(this);
    }
}