using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

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
            if (_activated) return;
            _activated = true;
            _spriteRenderer.sprite = _enabledSprite;
            _activationEvent?.Invoke();
        }
    }

    [RequireComponent(typeof(PlatformEffector2D))]
    public class FallingGround : MonoBehaviour
    {
        [SerializeField] private PlatformEffector2D _effector;
        [SerializeField] private TilemapCollider2D _collider;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            var player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
            }
        }
    }
}