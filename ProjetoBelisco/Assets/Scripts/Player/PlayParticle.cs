using UnityEngine;

namespace GameScripts.Player
{
    public class PlayParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;



        public void EmitParticle()
        {
            _particleSystem.Play();
        }
    }
}
