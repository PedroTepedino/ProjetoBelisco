﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerAnimatorController : MonoBehaviour
    {
        [BoxGroup("RIGHT")] [SerializeField] private Animator _animatorRight;
        [BoxGroup("RIGHT")] [SerializeField] private SpriteRenderer _spriteRendererRight;

        [BoxGroup("LEFT")] [SerializeField] private Animator _animatorLeft;
        [BoxGroup("LEFT")] [SerializeField] private SpriteRenderer _spriteRendererLeft;

        private Player _player;

        private Rigidbody2D _rigidbody;

        public bool IsLookingRight { get; private set; } = true;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRendererRight.enabled = true;
            _spriteRendererLeft.enabled = false;
            _player = GetComponent<Player>();
        }

        public void UpdateParameters(bool touchingGround)
        {
            var horizontalVelocity = _rigidbody.velocity.x;

            if (horizontalVelocity > 0.1f)
                IsLookingRight = true;
            else if (horizontalVelocity < -0.1f) IsLookingRight = false;

            ExitDashAnimation(!_player.IsDashing && Mathf.Abs(RewiredPlayerInput.Instance.Horizontal) > 0.1f);

            LookDirection(IsLookingRight);

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

        public void Glide(bool glide)
        {
            SetBool("Gliding", glide);
        }

        public void HoldWall(bool holdWall)
        {
            SetBool("TouchingWall", holdWall);
        }

        public void ExitDashAnimation(bool endDash)
        {
            SetBool("ExitDash", endDash);
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

        public void Hurt()
        {
            SetTrigger("Hit");
        }
    }
}