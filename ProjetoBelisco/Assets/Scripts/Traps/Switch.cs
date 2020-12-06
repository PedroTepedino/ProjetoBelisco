using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Belisco
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
        private bool _activated;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _disabledSprite;
            _activated = false;
        }

        // TODO: add knockback if necessary
        public void Hit(int damage, Transform attacker)
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