using Player;
using UnityEngine;

public interface IDamageable
{
    Vector3 GetPosition();
    EFaction GetFaction();
    void TakeDamage(int damage);
}