using UnityEngine;

namespace GameScripts.Player
{
    public class PlayParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private RefatoramentoDoTioTepe.Player _player;

        private void Awake()
        {
            _player.OnTouchedGround += EmitParticle;
        }

        private void OnDestroy()
        {
            _player.OnTouchedGround -= EmitParticle;
        }

        public void EmitParticle()
        {
            _particleSystem.Play();
        }

        private void OnValidate()
        {
            if (_player == null)
            {
                _player = this.GetComponentInParent<RefatoramentoDoTioTepe.Player>();
            }
        }
    }
}
