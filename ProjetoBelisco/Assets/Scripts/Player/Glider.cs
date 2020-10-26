using UnityEngine;

namespace Belisco
{
    public class Glider
    {
        private readonly float _glideFallVelocity;
        private readonly Grounder _grounder;
        private readonly PlayerAnimatorController _playerAnimator;
        private readonly Rigidbody2D _rigidbody;

        public Glider(Player player)
        {
            _rigidbody = player.GetComponent<Rigidbody2D>();
            _grounder = player.Grounder;
            _playerAnimator = player.AnimatorController;
            _glideFallVelocity = player.PlayerParameters.GlideFallVelocity;
        }

        public bool Gliding { get; private set; }

        public void Tick()
        {
            if (!_grounder.IsGrounded && RewiredPlayerInput.Instance.InitiateJump)
            {
                if (_rigidbody.velocity.y < 0f) StartGlide();
            }
            else if (_grounder.IsGrounded || RewiredPlayerInput.Instance.TerminateJump)
            {
                EndGlider();
            }

            if (Gliding) _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _glideFallVelocity);
        }

        private void StartGlide()
        {
            Gliding = true;
            _playerAnimator.Glide(true);
        }

        public void EndGlider()
        {
            Gliding = false;
            _playerAnimator.Glide(false);
        }
    }
}