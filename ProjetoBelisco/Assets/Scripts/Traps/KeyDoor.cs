using DG.Tweening;
using UnityEngine;

namespace Belisco
{
    public class KeyDoor : AbstractLockable
    {
        private Tween _animation;

        private void OnEnable()
        {
            _animation = transform.DOScale(1f, 0.5f).SetAutoKill(false).From(0f).SetEase(Ease.OutBack);
            _animation.Play();
            _animation.onRewind += () => gameObject.SetActive(false);
            _animation.onRewind += () => _animation.Kill();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerKeysManager keyManager = other.GetComponent<PlayerKeysManager>();
                if (keyManager.HasKey)
                {
                    keyManager.UseKey();
                    Unlock();
                }
            }
        }

        public override void Lock()
        {
        }

        public override void Unlock()
        {
            _animation.SmoothRewind();
        }
    }
}