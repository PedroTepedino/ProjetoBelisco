using UnityEngine;

namespace Belisco
{
    public class PlayParticlesGeneral : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public void EmitParticle()
        {
            _particleSystem.Play();
        }
    }
}