using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnDeadCourpse : MonoBehaviour
{
    private EnemyLife _enemyLife;
    private EnemyController _enemyController;

    [SerializeField] private string _deadBodyTag;

    private void Awake()
    {
        _enemyLife = this.GetComponent<EnemyLife>();
        _enemyController = this.GetComponent<EnemyController>();

        _enemyLife.OnEnemyDie += SpawnBody;
    }

    private void OnDestroy()
    {
        _enemyLife.OnEnemyDie -= SpawnBody;
    }

    private void SpawnBody()
    {
        GameObject body = ObjectPooler.Instance.SpawnFromPool(_deadBodyTag, this.transform.position,
            _enemyController.movingRight ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity);
    }
}
