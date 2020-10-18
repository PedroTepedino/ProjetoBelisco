using System;
using System.Collections;
using System.Data;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
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
        [ShowInInspector] private Vector3 _initialPosition;

        private WaitForSeconds _waitForTimeToFall;
        private WaitForSeconds _waitForTimeToDisappear;
        private WaitForSeconds _waitForTimeToRespawn;

        private Tween _appearAnimation;

        private void Awake()
        {
            _initialPosition = this.transform.position;
            _waitForTimeToFall = new WaitForSeconds(_timeToFall);
            _waitForTimeToDisappear = new WaitForSeconds(_timeToDisappear);
            _waitForTimeToRespawn = new WaitForSeconds(_timeToRespawn);

            _appearAnimation = this.transform.DOScale(1f, 0.5f).From(0f).SetAutoKill(false).SetEase(Ease.OutBack);
            _appearAnimation.Play();

            LockPlatform();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(FallCoroutine());
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                
            }
        }

        private IEnumerator FallCoroutine()
        {
            yield return _waitForTimeToFall;
            
            UnlockPlatform();

            yield return _waitForTimeToDisappear;

            _appearAnimation.SmoothRewind();
            _collider.enabled = false;

            yield return _waitForTimeToRespawn;
            
            this.transform.position = _initialPosition;
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
        

        private void OnValidate()
        {
            if (_rigidbody == null)
            {
                _rigidbody = this.GetComponent<Rigidbody2D>();
            }

            if (_collider == null)
            {
                _collider = this.GetComponent<Collider2D>();
            }
        }
    }
}