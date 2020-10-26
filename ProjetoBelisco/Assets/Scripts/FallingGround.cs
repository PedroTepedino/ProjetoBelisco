using DG.Tweening;
using UnityEngine;

namespace Belisco
{
    [RequireComponent(typeof(Collider2D))]
    public class FallingGround : MonoBehaviour
    {
        [SerializeField] private DOTweenAnimation _shakeAnimation;
        [SerializeField] private Transform _platform;

        [SerializeField] private float _timeToFall = 2.5f;

        [SerializeField] private float _timeToRespawn = 3f;
        private Collider2D _collider;
        private bool _colliding;
        private Tween _fallAnimation;
        private float _respawnTimer;
        private float _timer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _timer = _timeToFall;
            _fallAnimation = _platform.DOMove(_platform.position + 5 * Vector3.down, 1f)
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
    }
}