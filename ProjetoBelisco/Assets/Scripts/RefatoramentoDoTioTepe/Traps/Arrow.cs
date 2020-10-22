using DG.Tweening;
using GameScripts.PoolingSystem;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    // [RequireComponent(typeof(Collider2D))]
    // [RequireComponent(typeof(Rigidbody2D))]
    public class Arrow : MonoBehaviour, IPooledObject
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _velocity;
        [SerializeField] private int _damage = 1;

        private Tween _animation;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var hittable = other.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.Hit(_damage);
            }

            _rigidbody.velocity = Vector2.zero;

            _animation.SmoothRewind();
        }

        public void OnObjectSpawn(object[] parameters = null)
        {
            _animation = this.transform.DOScale(1f, 0.2f).From(0f).SetAutoKill(false).SetEase(Ease.InOutBack);
            _animation.onRewind += () => _animation.Kill();
            _animation.onRewind += () => this.gameObject.SetActive(false);
            _rigidbody.velocity = this.transform.up * _velocity;
        }

        private void OnValidate()
        {
            //this.GetComponent<Collider2D>().isTrigger = true;
            if (_rigidbody == null)
            {
                _rigidbody = this.GetComponent<Rigidbody2D>();
            }
        }
    }
}