using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        //string aux = null;
        // if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        // {
        //     Debug.Log( "Run");    
        // }
        // if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        // {
        //     Debug.Log( "Jump");    
        // }
        // if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Ascending"))
        // {
        //     Debug.Log( "Ascend");    
        // }
        // if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Contact"))
        // {
        //     Debug.Log( "contact");    
        // }
    }

    private void SetAnimatorValues()
    {
        _animator.SetFloat("SpeedX", Mathf.Abs(_rigidbody.velocity.x));
        _animator.SetFloat("SpeedY", _rigidbody.velocity.y);
        _spriteRenderer.flipX = !PlayerMovement.IsLookingRight;
        _animator.SetBool("Jumping", PlayerJump.IsJumping);
        _animator.SetBool("Falling", !PlayerGrounder.IsTouchingGround);
    }
        
    private void TriggerAttackAnimation()
    {
        _animator.SetTrigger("Attack");
        _animator.SetInteger("Combo", _comboManager.CurrentComboCount);
    }
}
