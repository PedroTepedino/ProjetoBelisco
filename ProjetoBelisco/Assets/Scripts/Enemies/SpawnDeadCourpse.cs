using GameScripts.PoolingSystem;
using UnityEngine;

namespace GameScripts.Enemies
{
    public class SpawnDeadCourpse : MonoBehaviour
    {
        private Life _life;
        private Controller _controller;

        [SerializeField] private string _deadBodyTag;

        private void Awake()
        {
            _life = this.GetComponent<Life>();
            _controller = this.GetComponent<Controller>();

            _life.OnEnemyDie += SpawnBody;
        }

        private void OnDestroy()
        {
            _life.OnEnemyDie -= SpawnBody;
        }

        private void SpawnBody()
        {
            GameObject body = Pooler.Instance.SpawnFromPool(_deadBodyTag, this.transform.position,
                _controller.movingRight ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity);
        }
    }
}
