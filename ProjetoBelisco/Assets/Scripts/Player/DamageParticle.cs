using UnityEngine;

namespace GameScripts.Player
{
    public class DamageParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _damageParticles;

        private void Awake()
        {
            Life.OnPlayerDamage += PlayParticle;
        }

        private void OnDestroy()
        {
            Life.OnPlayerDamage -= PlayParticle;
        }

        private void PlayParticle(int damage, int totalHealth)
        {
            _damageParticles.Play();
        }
    }
}