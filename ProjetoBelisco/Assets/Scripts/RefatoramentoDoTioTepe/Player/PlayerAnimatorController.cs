using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerAnimatorController : MonoBehaviour
    {
        [BoxGroup("RIGHT")] [SerializeField] private Animator _animatorRight;
        [BoxGroup("RIGHT")] [SerializeField] private SpriteRenderer _spriteRendererRight;
        
        [BoxGroup("LEFT")] [SerializeField] private Animator _animatorLeft;
        [BoxGroup("LEFT")] [SerializeField] private SpriteRenderer _spriteRendererLeft;
        
        private Rigidbody2D _rigidbody;
        private Player _player;

        private void Awake()
        {
            _rigidbody = this.GetComponent<Rigidbody2D>();
            _spriteRendererRight.enabled = true;
            _spriteRendererLeft.enabled = false;
            _player = this.GetComponent<Player>();
            _player.OnJump += Jump;
        }

        private void OnDestroy()
        {
            _player.OnJump -= Jump;
        }

        public void UpdateParameters(bool touchingGround)
        {
            float horizontalVelocity = _rigidbody.velocity.x;
            
            if (horizontalVelocity > 0f)
            {
                LookDirection(lookRight: true);
            }
            else if (horizontalVelocity < 0f)
            {
                LookDirection(lookRight: false);
            }
            
            SetFloat("HorizontalVelocity", Mathf.Abs(horizontalVelocity));
            SetFloat("VerticalVelocity", _rigidbody.velocity.y);
            SetBool("TouchingGround", touchingGround);
        }

        private void SetFloat(string floatName, float value)
        {
            _animatorRight.SetFloat(floatName, value);
            _animatorLeft.SetFloat(floatName, value);
        }

        private void SetBool(string boolName, bool value)
        {
            _animatorRight.SetBool(boolName, value);
            _animatorLeft.SetBool(boolName, value);
        }

        private void SetTrigger(string triggerName)
        {
            _animatorRight.SetTrigger(triggerName);
            _animatorLeft.SetTrigger(triggerName);
        }

        private void Jump()
        {
            SetTrigger("Jump");
        }

        public void StrongAttack()
        {
            SetTrigger("StrongAttack");
        }

        private void LookDirection(bool lookRight)
        {
            _spriteRendererRight.enabled = lookRight;
            _spriteRendererLeft.enabled = !lookRight;
        }
    }
}