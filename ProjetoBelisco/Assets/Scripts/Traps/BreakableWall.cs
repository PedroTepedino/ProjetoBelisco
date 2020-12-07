using DG.Tweening;
using UnityEngine;

namespace Belisco
{
    public class BreakableWall : MonoBehaviour, IHittable
    {
        [SerializeField] private int _hitsUntilBreak = 3;
        private int _currentHitsCount;

        private Tween _disappearAnimation;
        private Tween _hitAnimation;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _currentHitsCount = 0;

            _disappearAnimation = transform.DOScale(0f, 0.2f).From(new Vector3(1f, 1f, 1f)).SetEase(Ease.InBack);
            _disappearAnimation.Rewind();
            _disappearAnimation.onComplete += () => gameObject.SetActive(false);

            _hitAnimation = _spriteRenderer.DOColor(Color.white, 0.2f)
                .From(_spriteRenderer.color)
                .SetLoops(2, LoopType.Yoyo)
                .SetAutoKill(false)
                .SetEase(Ease.Flash);
            _hitAnimation.Rewind();
        }

        // TODO: add knockback if necessary
        public void Hit(int damage, Transform attacker)
        {
            _currentHitsCount++;
            _hitAnimation.Restart();

            if (_currentHitsCount >= _hitsUntilBreak)
            {
                _hitAnimation.Rewind();
                _hitAnimation.Kill();
                _disappearAnimation.Play();
            }
        }
    }
}