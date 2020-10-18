using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Switch : MonoBehaviour, IHittable
    {
        [SerializeField] [AssetSelector] [AssetsOnly]
        private Sprite _disabledSprite;
        [SerializeField] [AssetSelector] [AssetsOnly]
        private Sprite _enabledSprite;

        [SerializeField] private UnityEvent _activationEvent;
        
        private SpriteRenderer _spriteRenderer;
        private bool _activated;

        private void Awake()
        {
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _disabledSprite;
            _activated = false;
        }
        
        public void Hit(int damage)
        {
            ActivateSwitch();
        }

        private void ActivateSwitch()
        {
            _activated = true;
            _spriteRenderer.sprite = _enabledSprite;
            _activationEvent?.Invoke();
        }
    }
}