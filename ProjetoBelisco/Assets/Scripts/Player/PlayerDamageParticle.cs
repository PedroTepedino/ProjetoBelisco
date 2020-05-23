using UnityEngine;

public class PlayerDamageParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _damageParticles;

    private void Awake()
    {
        PlayerLife.OnPlayerDamage += PlayParticle;
    }

    private void OnDestroy()
    {
        PlayerLife.OnPlayerDamage -= PlayParticle;
    }

    private void PlayParticle(int damage, int totalHealth)
    {
        _damageParticles.Play();
    }
}
