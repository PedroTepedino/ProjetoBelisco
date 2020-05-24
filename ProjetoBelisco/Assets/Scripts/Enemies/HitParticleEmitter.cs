using UnityEngine;

namespace GameScripts.Enemies
{
    public class HitParticleEmitter : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleEmitter;
        private Life _life;

        private void Awake()
        {
            _life = this.GetComponent<Life>();
            _life.OnEnemyDamage += ListenOnDamage;
        }

        private void OnDestroy()
        {
            _life.OnEnemyDamage -= ListenOnDamage;
        }

        private void ListenOnDamage(int damage, int maxLife)
        {
            _particleEmitter.Play();
        }
    }
}
