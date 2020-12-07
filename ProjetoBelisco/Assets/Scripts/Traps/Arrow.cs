using DG.Tweening;
using Rewired;
using UnityEngine;

namespace Belisco
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
            IHittable hittable = other.GetComponent<IHittable>();
            if (hittable != null) hittable.Hit(_damage, this.transform);

            _rigidbody.velocity = Vector2.zero;

            _animation.SmoothRewind();
        }

        private void OnValidate()
        {
            //this.GetComponent<Collider2D>().isTrigger = true;
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void OnObjectSpawn(object[] parameters = null)
        {
            _animation = transform.DOScale(1f, 0.2f).From(0f).SetAutoKill(false).SetEase(Ease.InOutBack);
            _animation.onRewind += () => _animation.Kill();
            _animation.onRewind += () => gameObject.SetActive(false);
            _rigidbody.velocity = transform.up * _velocity;
        }
    }
}