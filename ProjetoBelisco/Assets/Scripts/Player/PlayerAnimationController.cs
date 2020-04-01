﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private PlayerComboManager _comboManager;

    private void Awake()
    {
        _rigidbody = this.GetComponent<Rigidbody2D>();
        _animator = this.GetComponentInChildren<Animator>();
        _spriteRenderer = _animator.gameObject.GetComponent<SpriteRenderer>();
        _comboManager = this.GetComponent<PlayerComboManager>();
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
}
