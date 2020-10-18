using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Jumper : IJumper
    {
        private readonly Player _player;
        private Rigidbody2D _rigidbody;
        private readonly Grounder _playerGrounder;
        private readonly float _jumpVelocity;
        private readonly float _maxButtonHoldTime;
        private float _jumpTimer;
        private readonly float _gravityFallMultiplayer;
        private Vector2 _velocityWhenTerminateJump;

        private bool _canJumpAgain = true;
        
        public bool Jumping { get; private set; }

        public Jumper(Player player)
        {
            _player = player;
            _rigidbody = player.gameObject.GetComponent<Rigidbody2D>();
            _playerGrounder = player.Grounder;
            _jumpVelocity = player.PlayerParameters.JumpVelocity;
            _maxButtonHoldTime = player.PlayerParameters.MaxJumpHoldTime;
            _gravityFallMultiplayer = player.PlayerParameters.GravityFallMultiplayer;
            _velocityWhenTerminateJump = new Vector2(0f, player.PlayerParameters.VelocityWhenTerminateJump);
        }

        public void Tick()
        {
            Vector2 velocity = _rigidbody.velocity;
            
            float timerFactor = 1 - (_jumpTimer / _maxButtonHoldTime);

            if (RewiredPlayerInput.Instance.Jump)
            {
                if (((_playerGrounder.IsGrounded && _canJumpAgain) || Jumping))
                {
                    if ((_jumpTimer <= _maxButtonHoldTime))
                    {
                        velocity.y = _jumpVelocity * timerFactor;
                        Jumping = true;
                        _jumpTimer += Time.deltaTime;
                        _canJumpAgain = false;
                    }
                }
            }
            else
            {
                _jumpTimer = 0f;
                Jumping = false;
            }

            _rigidbody.gravityScale = velocity.y < -0.1f ? _gravityFallMultiplayer : 1f;

            if (RewiredPlayerInput.Instance.TerminateJump)
            {
                if (_rigidbody.velocity.y > _jumpVelocity / 2f)
                {
                    _velocityWhenTerminateJump.x = _rigidbody.velocity.x;
                    velocity = _velocityWhenTerminateJump;
                }
                
                _canJumpAgain = true;
            }

            if (RewiredPlayerInput.Instance.InitiateJump)
            {
                // If near ground Can jump, otherwise can't
                _canJumpAgain = _playerGrounder.NearGround;
            }
            
            _rigidbody.velocity = velocity;
        }
    }
}