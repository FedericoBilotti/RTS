using UnityEngine;

namespace Units
{
    public interface ITargetable : IDamageable, IFaction
    {
        Vector3 GetPosition();
    }
}