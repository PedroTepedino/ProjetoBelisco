using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private PlayerComboManager _comboManager;

    public static Action OnAttackAnimationEnd;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody2D>();
        _animator = this.GetComponentInChildren<Animator>();
        _spriteRenderer = _animator.gameObject.GetComponent<SpriteRenderer>();
        _comboManager = this.GetComponent<PlayerComboManager>();
        PlayerAttackSystem.OnAttack += TriggerAttackAnimation;
    }

    private void OnDestroy()
    {
        PlayerAttackSystem.OnAttack -= TriggerAttackAnimation;
    }

    private void Update()
    {
        SetAnimatorValues();
    }

    private void SetAnimatorValues()
    {
        _animator.SetFloat("SpeedX", Mathf.Abs(_rigidbody.velocity.x));
        _animator.SetFloat("SpeedY", _rigidbody.velocity.y);
        _spriteRenderer.flipX = !PlayerMovement.IsLookingRight;
        _animator.SetBool("Jumping", !PlayerGrounder.IsTouchingGround);
    }
        
    private void TriggerAttackAnimation()
    {
        _animator.SetTrigger("Attack");
        _animator.SetInteger("Combo", _comboManager.CurrentComboCount);
    }
}
