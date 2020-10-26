using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    public class Ghost : MonoBehaviour, IPooledObject
    {
        private static readonly int FadeAmount = Shader.PropertyToID("_FadeAmount");
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _initialOpacity = 0.5f;

        [BoxGroup("Tween Parameters")] [SerializeField]
        private float _timeToFade = 1f;

        [BoxGroup("Tween Parameters")] [SerializeField]
        private Ease _easeType = Ease.Linear;

        private void OnValidate()
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnObjectSpawn(object[] parameters = null)
        {
            if (parameters != null) SetSprite(parameters.ToList().Find(obj => obj is Sprite) as Sprite);

            DOTween.To(() => _spriteRenderer.material.GetFloat(FadeAmount),
                    amount => _spriteRenderer.material.SetFloat(FadeAmount, amount), 1, 1f)
                .From(0f);
        }

        private void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        private void DisableOnComplete()
        {
            gameObject.SetActive(false);
        }
    }
}