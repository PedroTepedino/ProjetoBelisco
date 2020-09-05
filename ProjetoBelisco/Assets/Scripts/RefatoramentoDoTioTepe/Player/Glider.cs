using Rewired;
using UnityEngine;
using UnityEngine.UIElements;

namespace RefatoramentoDoTioTepe
{
    public class Glider
    {
        private Rigidbody2D _rigidbody;
        private readonly Grounder _grounder;
        private PlayerAnimatorController _playerAnimator;
        private bool _gliding = false;
        private readonly float _glideFallVelocity;

        public bool Gliding => _gliding;

        public Glider(Player player)
        {
            _rigidbody = player.GetComponent<Rigidbody2D>();
            _grounder = player.Grounder;
            _playerAnimator = player.AnimatorController;
            _glideFallVelocity = player.PlayerParameters.GlideFallVelocity;
        }

        public void Tick()
        {
            if (!_grounder.IsGrounded && RewiredPlayerInput.Instance.InitiateJump)
            {
                if (_rigidbody.velocity.y < 0f)
                {
                    StartGlide();
                }
            }
            else if (_grounder.IsGrounded || RewiredPlayerInput.Instance.TerminateJump)
            {
                EndGlider();
            }

            if (_gliding)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _glideFallVelocity);
            }
        }

        private void StartGlide()
        {
            _gliding = true;
            _playerAnimator.Glide(true);
        }

        public void EndGlider()
        {
            _gliding = false;
            _playerAnimator.Glide(false);
        }
    }
}