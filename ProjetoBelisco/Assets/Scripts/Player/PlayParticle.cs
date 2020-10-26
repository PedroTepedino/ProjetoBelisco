using UnityEngine;

namespace Belisco
{
    public class PlayParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private Player _player;

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
        }

        private void OnEnable()
        {
            _player.OnTouchedGround += EmitParticle;
        }

        private void OnDisable()
        {
            _player.OnTouchedGround -= EmitParticle;
        }

        public void EmitParticle()
        {
            _particleSystem.Play();
        }
    }
}