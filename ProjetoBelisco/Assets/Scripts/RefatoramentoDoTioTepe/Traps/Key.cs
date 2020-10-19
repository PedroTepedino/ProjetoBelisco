using DG.Tweening;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Key : MonoBehaviour
    {
        private Tween _animation;
        
        private void OnEnable()
        {
            _animation = this.transform.DOScale(1f, 0.5f).From(0f).SetAutoKill(false).SetEase(Ease.OutBack);
            _animation.Play();
            _animation.onRewind += () => this.gameObject.SetActive(false);
            _animation.onRewind += () => _animation.Kill();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerKeysManager>().AddKey(this);
                _animation.SmoothRewind();
            }
        }
    }
}