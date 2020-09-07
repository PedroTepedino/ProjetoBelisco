using System;
using DG.Tweening;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Collider2D))]
    public class FallingGround : MonoBehaviour
    {
        [SerializeField] private DOTweenAnimation _shakeAnimation;
        [SerializeField] private Transform _platform ;
        private Collider2D _collider;
        private Tween _fallAnimation;

        [SerializeField] private float _timeToFall = 2.5f;
        private float _timer;

        [SerializeField] private float _timeToRespawn = 3f;
        private float _respawnTimer;
        private bool _colliding;

        private void Awake()
        {
            _collider = this.GetComponent<Collider2D>();
            _timer = _timeToFall;
            _fallAnimation = _platform.DOMove(_platform.position + (5 * Vector3.down), 1f)
                .SetEase(Ease.OutExpo)
                .SetAutoKill(false)
                .OnComplete(() => _platform.gameObject.SetActive(false))
                .OnRewind(() => _platform.gameObject.SetActive(true));
            _fallAnimation.Rewind();
        }

        private void Update()
        {
            if (_timer <= 0f && _collider.enabled)
            {
                Fall();
            }

            if (!_collider.enabled)
            {
                if (_respawnTimer <= 0f)
                {
                    Respawn();          
                }
                _respawnTimer -= Time.deltaTime;
            }

            if (_colliding)
            {
                _timer -= Time.deltaTime;
            }
        }

        private void Fall()
        {
            _fallAnimation.Play();
            _collider.enabled = false;
            _respawnTimer = _timeToRespawn;
            _timer = _timeToFall;
        }

        private void Respawn()
        {
            _timer = _timeToFall;
            _fallAnimation.Rewind();
            _collider.enabled = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Player>())
            {
                _timer = _timeToFall;
                _shakeAnimation.DOPlay();
                _colliding = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Player>())
            {
                _shakeAnimation.DORewind();
                _colliding = false;
            }
        }
    }
}