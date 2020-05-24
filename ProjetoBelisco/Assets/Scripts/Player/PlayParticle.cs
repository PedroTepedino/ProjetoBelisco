using UnityEngine;

namespace GameScripts.Player
{
    public class PlayParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;



        private void Awake()
        {
            Grounder.OnTouchGround += EmitParticle;
        }

        private void OnDestroy()
        {
            Grounder.OnTouchGround -= EmitParticle;
        }

        public void EmitParticle()
        {
            _particleSystem.Play();
        }
    }
}
