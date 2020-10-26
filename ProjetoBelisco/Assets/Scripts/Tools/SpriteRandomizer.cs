#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRandomizer : MonoBehaviour
    {
        [SerializeField] private Sprite[] _spriteList;
        [SerializeField] private bool _isRandom = true;
        [SerializeField] [HideIf("IsRandom")] private int _spriteIndex;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public bool IsRandom => _isRandom;

        private void OnEnable()
        {
            if (_isRandom)
                RandomizeSprite();
        }

        private void OnValidate()
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();

            if (!_isRandom)
                if (_spriteIndex > 0 && _spriteIndex < _spriteList.Length)
                    _spriteRenderer.sprite = _spriteList[_spriteIndex];
        }

        [Button(ButtonSizes.Gigantic)]
        [ShowIf("IsRandom")]
        public void RandomizeSprite()
        {
            _spriteRenderer.sprite = _spriteList[Random.Range(0, _spriteList.Length)];
        }
    }
}

#endif //UNITY_EDITOR