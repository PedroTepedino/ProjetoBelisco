﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Grounder))]
    [RequireComponent(typeof(Collider2D))]
    public class Player : MonoBehaviour, IHittable
    {
        [SerializeField] [AssetsOnly] [InlineEditor(InlineEditorObjectFieldModes.Hidden)] private PlayerParameters _playerParameters;
        private IMover _mover;
        private IJumper _jumper;
        private IAttacker _basicAttacker;
        private LifeSystem _lifeSystem;
        private Grounder _grounder;
        
        [SerializeField] 
        private PlayerAnimatorController _playerAnimatorController;

        private bool _lastFrameJumpState = false;

        public event Action OnJump;

        public PlayerParameters PlayerParameters => _playerParameters;
        public LifeSystem LifeSystem => _lifeSystem;
        public Grounder Grounder => _grounder;
        public bool Jumping => _jumper.Jumping;

        private void Awake()
        {
            _grounder = new Grounder(this);
            _lifeSystem = new LifeSystem(this);
            _mover = new Mover(this);
            _jumper = new Jumper(this);
            _basicAttacker = new BasicAttacker(this);
            
        }

        public void Hit(int damage)
        {
            _lifeSystem.Damage(damage);
        }

        private void Update()
        {
            _mover.Tick();
            _jumper.Tick();
            _basicAttacker.Tick();
            
            CheckJumping();
            _playerAnimatorController.UpdateParameters(_grounder.IsGrounded);
        }

        private void OnValidate()
        {
            if (_playerParameters == null)
                _playerParameters = Resources.Load<PlayerParameters>("ScriptableObjects/DefaultPlayerParameters");

            if (_playerAnimatorController == null)
                _playerAnimatorController = this.GetComponent<PlayerAnimatorController>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(this.transform.position + _playerParameters.GrounderPosition, _playerParameters.GrounderSizes);
            
            
        }

        private void CheckJumping()
        {
            if (!_lastFrameJumpState && _jumper.Jumping)
            {
                OnJump?.Invoke();
            }

            _lastFrameJumpState = _jumper.Jumping;
        }
    }
}
