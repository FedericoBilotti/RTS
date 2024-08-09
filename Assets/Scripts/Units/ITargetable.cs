using Player;
using UnityEngine;

namespace Units
{
    public interface ITargetable : IDamageable
    {
        Vector3 GetPosition();
        EFaction GetFaction();
    }
}