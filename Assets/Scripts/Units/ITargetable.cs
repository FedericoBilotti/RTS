using Player;
using UnityEngine;

namespace Units
{
    public interface ITargetable
    {
        Vector3 GetPosition();
        EFaction GetFaction();
        bool IsDead();
    }
}