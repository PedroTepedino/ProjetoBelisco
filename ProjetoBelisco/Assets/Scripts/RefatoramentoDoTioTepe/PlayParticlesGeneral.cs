using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class PlayParticlesGeneral : MonoBehaviour
    {
        [SerializeField] ParticleSystem _particleSystem;

        public void EmitParticle()
        {
            _particleSystem.Play();
        }
    }
}