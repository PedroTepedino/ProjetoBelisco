using GameScripts.PoolingSystem;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlePooledObject : MonoBehaviour, IPooledObject
    {
        [SerializeField] private ParticleSystem _particleSystem;
        public void OnObjectSpawn(object[] parameters = null)
        {
            _particleSystem.Play();
        }

        private void OnValidate()
        {
            if (_particleSystem == null)
            {
                _particleSystem = this.GetComponent<ParticleSystem>();
            }
        }
    }
}