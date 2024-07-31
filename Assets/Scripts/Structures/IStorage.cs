using UnityEngine;

namespace Structures
{
    public interface IStorage
    {
        Vector3 Position { get; } 
        StorageTypes GetStorageType();
    }

    public enum StorageTypes
    {
        Center,
        Wood,
        Gold,
        Food
    }
}