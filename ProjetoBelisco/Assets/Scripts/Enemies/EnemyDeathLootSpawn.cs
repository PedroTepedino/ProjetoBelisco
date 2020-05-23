using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyDeathLootSpawn : MonoBehaviour
{
    [SerializeField][MinMaxSlider(0, 50)] private Vector2Int _minMaxCoinDrop;

    [SerializeField] private string _pawnTag = "Paw";
    
    private EnemyLife _enemyLife;

    private void Awake()
    {
        _enemyLife = this.GetComponent<EnemyLife>();
        _enemyLife.OnEnemyDie += ListenOnDeath;
    }

    private void OnDestroy()
    {
        _enemyLife.OnEnemyDie -= ListenOnDeath;
    }

    private void ListenOnDeath()
    {
        int rate = Random.Range(_minMaxCoinDrop.x, _minMaxCoinDrop.y + 1);
        GameObject aux = null;
        for (int i = 0; i < rate; i++)
        {
            aux = ObjectPooler.Instance.SpawnFromPool(_pawnTag, this.transform.position, Quaternion.identity);   
            aux.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100f, 100f), Random.Range(100f, 300f)));
        }
    }
}
