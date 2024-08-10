using Units.Resources.ScriptableObjects;
using UnityEngine;

namespace Units.Resources
{
    public interface IWork
    {
        Vector3 Position { get; }
        bool HasResources();
        int ProvideResource();
        void PlayAnimation(Unit unit);
        ResourceSO GetResourceSO();
    }
}