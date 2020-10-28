using UnityEngine;

namespace Belisco
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticlePooledObject : MonoBehaviour, IPooledObject
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private void OnValidate()
        {
            if (_particleSystem == null) _particleSystem = GetComponent<ParticleSystem>();
        }

        public void OnObjectSpawn(object[] parameters = null)
        {
            _particleSystem.Play();
        }
    }
}