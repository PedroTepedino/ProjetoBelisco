using GameScripts.PoolingSystem;
using UnityEngine;

namespace GameScripts.Player
{
    public class PoolableObject : MonoBehaviour, IPooledObject
    {
        private Life _life;

        private void Awake()
        {
            _life = this.GetComponent<Life>();
        }

        public void OnObjectSpawn()
        {
            _life.RespawnPlayer();
        }
    }
}
