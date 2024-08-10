using Manager;
using Player;
using Units;
using UnityEngine;

namespace Structures.Storages
{
    public interface IStorage : IFaction
    {
        Vector3 Position { get; }
        ResourcesManager.ResourceType StorageType { get; }
    }
}