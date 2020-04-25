using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : BaseLifeSystem
{
    public static System.Action<int, int> OnEnemyDamage;
    public static System.Action OnEnemyDie;
    public static System.Action<int, int> OnEnemyHeal;

    public override void Damage(int damagePoints = 1)
    {
        if (damagePoints <= 0) return;
        
        _curentHealthPoints -= damagePoints;
        OnEnemyDamage?.Invoke(_curentHealthPoints, _maximumHealth);

        if (IsDead)
        {
            Die();
        }
    }

    public override void RestoreHealth(int healPoints = 1)
    {
        if (healPoints <= 0) return;
        
        _curentHealthPoints += healPoints;
        OnEnemyHeal?.Invoke(_curentHealthPoints, _maximumHealth);

        if (IsHealthFull)
        {
            _curentHealthPoints = _maximumHealth;
        }
    }

    protected override void Die(){
        OnEnemyDie?.Invoke();
        this.gameObject.SetActive(false);
    }
}
