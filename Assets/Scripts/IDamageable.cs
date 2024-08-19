using Units;

public interface IDamageable
{
    EntityLife GetEntity();
    void TakeDamage(int damage);
    bool IsDead();
}