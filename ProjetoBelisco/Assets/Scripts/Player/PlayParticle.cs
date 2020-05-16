using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;



    private void Awake()
    {
        PlayerGrounder.OnTouchGround += EmitParticle;
    }

    private void OnDestroy()
    {
        PlayerGrounder.OnTouchGround -= EmitParticle;
    }

    public void EmitParticle()
    {
        _particleSystem.Play();
    }
}
