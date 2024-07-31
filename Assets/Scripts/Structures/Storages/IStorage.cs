using Manager;
using UnityEngine;

namespace Structures.Storages
{
    public interface IStorage
    {
        Vector3 Position { get; } 
        ResourcesManager.ResourceType GetStorageType { get; }
    }
}