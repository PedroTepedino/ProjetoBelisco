using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class FallingPlatform : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private float _timeToFall = 0.1f;
        [SerializeField] private float _timeToDisappear = 2f;
        [SerializeField] private float _timeToRespawn = 2f;

        private Tween _appearAnimation;
        [ShowInInspector] private Vector3 _initialPosition;
        private WaitForSeconds _waitForTimeToDisappear;

        private WaitForSeconds _waitForTimeToFall;
        private WaitForSeconds _waitForTimeToRespawn;

        private void Awake()
        {
            _initialPosition = transform.position;
            _waitForTimeToFall = new WaitForSeconds(_timeToFall);
            _waitForTimeToDisappear = new WaitForSeconds(_timeToDisappear);
            _waitForTimeToRespawn = new WaitForSeconds(_timeToRespawn);

            _appearAnimation = transform.DOScale(1f, 0.5f).From(0f).SetAutoKill(false).SetEase(Ease.OutBack);
            _appearAnimation.Play();

            LockPlatform();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player")) StartCoroutine(FallCoroutine());
        }


        private void OnValidate()
        {
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();

            if (_collider == null) _collider = GetComponent<Collider2D>();
        }

        private IEnumerator FallCoroutine()
        {
            yield return _waitForTimeToFall;

            UnlockPlatform();

            yield return _waitForTimeToDisappear;

            _appearAnimation.SmoothRewind();
            _collider.enabled = false;

            yield return _waitForTimeToRespawn;

            transform.position = _initialPosition;
            LockPlatform();
            _appearAnimation.isBackwards = false;
            _appearAnimation.Play();
        }

        private void LockPlatform()
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            _collider.enabled = true;
        }

        private void UnlockPlatform()
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }
}