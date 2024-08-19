using Manager;
using UnityEngine;

namespace Units.Structures.Storages
{
    public interface IStorage : IFaction
    {
        Vector3 Position { get; }
        ResourcesManager.ResourceType StorageType { get; }
    }
}