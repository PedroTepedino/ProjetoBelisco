#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RefatoramentoDoTioTepe
{

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRandomizer : MonoBehaviour
    {
        [SerializeField] private Sprite[] _spriteList;
        [SerializeField] private bool _isRandom = true;
        [SerializeField] [HideIf("IsRandom")] private int _spriteIndex = 0;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public bool IsRandom => _isRandom;

        private void OnEnable()
        {
            if (_isRandom)
                RandomizeSprite();
        }

        [Button(ButtonSizes.Gigantic)]
        [ShowIf("IsRandom")]
        public void RandomizeSprite()
        {
            _spriteRenderer.sprite = _spriteList[Random.Range(0, _spriteList.Length)];
        }

        private void OnValidate()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = this.GetComponent<SpriteRenderer>();
            }

            if (!_isRandom)
            {
                if (_spriteIndex > 0 && _spriteIndex < _spriteList.Length)
                {
                    _spriteRenderer.sprite = _spriteList[_spriteIndex];
                }
            }
        }
    }
}

#endif //UNITY_EDITOR