using UnityEngine;

namespace Belisco
{
    public class PlayDashParticles : MonoBehaviour
    {
        [SerializeField] private GameObject particlesGameObj;
        private ParticleSystem[] dashParticleSystem;

        private void Awake()
        {
            dashParticleSystem = particlesGameObj.GetComponentsInChildren<ParticleSystem>();
        }

        public void EmitDashParticle()
        {
            for (var i = 0; i < dashParticleSystem.Length; i++) dashParticleSystem[i].Play();
        }

        public void StopDashParticles()
        {
            for (var i = 0; i < dashParticleSystem.Length; i++) dashParticleSystem[i].Stop();
        }
    }
}