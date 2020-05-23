using UnityEngine;

public class EnemyHitParticleEmitter : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleEmitter;
    private EnemyLife _enemyLife;

    private void Awake()
    {
        _enemyLife = this.GetComponent<EnemyLife>();
        _enemyLife.OnEnemyDamage += ListenOnDamage;
    }

    private void OnDestroy()
    {
        _enemyLife.OnEnemyDamage -= ListenOnDamage;
    }

    private void ListenOnDamage(int damage, int maxLife)
    {
        _particleEmitter.Play();
    }
}
