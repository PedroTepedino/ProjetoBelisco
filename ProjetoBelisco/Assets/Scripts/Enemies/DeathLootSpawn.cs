using GameScripts.PoolingSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameScripts.Enemies
{
    public class DeathLootSpawn : MonoBehaviour
    {
        [SerializeField][MinMaxSlider(0, 50)] private Vector2Int _minMaxCoinDrop;

        [SerializeField] private string _pawnTag = "Paw";
    
        private Life _life;

        private void Awake()
        {
            _life = this.GetComponent<Life>();
            _life.OnEnemyDie += ListenOnDeath;
        }

        private void OnDestroy()
        {
            _life.OnEnemyDie -= ListenOnDeath;
        }

        private void ListenOnDeath()
        {
            int rate = Random.Range(_minMaxCoinDrop.x, _minMaxCoinDrop.y + 1);
            GameObject aux = null;
            for (int i = 0; i < rate; i++)
            {
                aux = Pooler.Instance.SpawnFromPool(_pawnTag, this.transform.position, Quaternion.identity);   
                aux.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100f, 100f), Random.Range(100f, 300f)));
            }
        }
    }
}
