using Manager;

namespace Units.Structures.Storages
{
    public class GoldStorage : Storage
     {
         private void Start()
         {
             StorageType = ResourcesManager.ResourceType.Gold;
         }
         
         private void OnEnable() => GameManager.Instance.AddStorage(this);
         private void OnDisable() => GameManager.Instance.RemoveStorage(this);
     }
 }