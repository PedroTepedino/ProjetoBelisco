using UnityEngine;

namespace Belisco
{
    public class PlayDashParticles : MonoBehaviour
    {
        [SerializeField] private GameObject particlesGameObj;
        private ParticleSystem[] dashParticleSystem;

        private void Awake()
        {
            if (dashParticleSystem != null)
            {
                dashParticleSystem = particlesGameObj.GetComponentsInChildren<ParticleSystem>();
            }
            else
            {
                dashParticleSystem = null;
            }
        }

        public void EmitDashParticle()
        {
            if (dashParticleSystem == null) return;
            
            for (var i = 0; i < dashParticleSystem.Length; i++) 
                dashParticleSystem[i]?.Play();
        }

        public void StopDashParticles()
        {
            if (dashParticleSystem == null) return;
            
            for (var i = 0; i < dashParticleSystem.Length; i++) 
                dashParticleSystem[i]?.Stop();
        }
    }
}