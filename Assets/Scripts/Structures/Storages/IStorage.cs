using Manager;
using Player;
using UnityEngine;

namespace Structures.Storages
{
    public interface IStorage
    {
        Vector3 Position { get; }
        ResourcesManager.ResourceType GetStorageType { get; }
        public EFaction Faction { get; }
        public void SetFaction(EFaction faction);
    }
}