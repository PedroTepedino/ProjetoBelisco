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

        private bool _isLookingRight = true;

        public bool IsLookingRight => _isLookingRight;

        private void Awake()
        {
            _rigidbody = this.GetComponent<Rigidbody2D>();
            _spriteRendererRight.enabled = true;
            _spriteRendererLeft.enabled = false;
            _player = this.GetComponent<Player>();
        }
        
        public void UpdateParameters(bool touchingGround)
        {
            float horizontalVelocity = _rigidbody.velocity.x;
            
            if (horizontalVelocity > 0.1f)
            {
                _isLookingRight = true;
            }
            else if (horizontalVelocity < -0.1f)
            {
                _isLookingRight = false;
            }
            
            LookDirection(lookRight: _isLookingRight);
            
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

        public void StrongAttack()
        {
            SetTrigger("StrongAttack");
        }

        public void Dash()
        {
            SetTrigger("Dash");
        }

        private void LookDirection(bool lookRight)
        {
            _spriteRendererRight.enabled = lookRight;
            _spriteRendererLeft.enabled = !lookRight;
        }

        public void Attack(Directions attackDirection)
        {
            switch (attackDirection)
            {
                case Directions.Left:
                case Directions.Right:
                    SetTrigger("Attack");
                    break;
                case Directions.Up:
                    SetTrigger("AttackUp");
                    break;
                case Directions.Down:
                    SetTrigger("AttackDown");
                    break;
            }
        }
    }
}