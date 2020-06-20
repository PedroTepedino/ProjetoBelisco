using System;
using Cinemachine;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Jumper : IJumper
    {
        private Rigidbody2D _rigidbody;
        private readonly Grounder _playerGrounder;
        private readonly float _jumpVelocity;
        private readonly float _maxButtonHoldTime;
        private float _jumpTimer;

        public bool Jumping { get; private set; }

        public Jumper(Player player)
        {
            _rigidbody = player.gameObject.GetComponent<Rigidbody2D>();
            _playerGrounder = player.Grounder;
            _jumpVelocity = player.PlayerParameters.JumpVelocity;
            _maxButtonHoldTime = player.PlayerParameters.MaxJumpHoldTime;
        }

        public void Tick()
        {
            Vector2 velocity = _rigidbody.velocity;

            if (RewiredPlayerInput.Instance.Jump && (_playerGrounder.IsGrounded || Jumping) && (_jumpTimer <= _maxButtonHoldTime))
            {
                velocity.y = _jumpVelocity;
                Jumping = true;
                _jumpTimer += Time.deltaTime;
            }
            else
            {
                _jumpTimer = 0f;
                Jumping = false;
            }
            
            _rigidbody.velocity = velocity;
        }
    }
}