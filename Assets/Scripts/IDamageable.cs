using UnityEngine;

public interface IDamageable
{
    Vector3 Position { get; }
    void TakeDamage(int damage);        
}