using System.Collections;
using UnityEngine;

namespace Belisco
{
    public class WallJumper
    {
        private readonly float _boxAngle;
        private readonly Vector2 _boxOffsetLeft;
        private readonly Vector2 _boxOffsetRight;
        private readonly Vector2 _boxSizes;
        private readonly float _fallingVelocity;
        private readonly Vector2 _jumpForce;
        private readonly LayerMask _layers;
        private readonly float _moverTimeOff;
        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private readonly PlayerAnimatorController _playerAnimator;
        private readonly Collider2D[] _raycastLeft = new Collider2D[2];

        private readonly Collider2D[] _raycastRight = new Collider2D[2];

        public WallJumper(Player player)
        {
            _player = player;
            _moverTimeOff = player.PlayerParameters.WallJumpMoverTimeOff;
            _boxOffsetRight = player.PlayerParameters.WallJumpBoxOffsetRight;
            _boxOffsetLeft = player.PlayerParameters.WallJumpBoxOffsetLeft;
            _boxSizes = player.PlayerParameters.WallJumpBoxSizes;
            _layers = player.PlayerParameters.WallJumpLayers;
            _boxAngle = player.PlayerParameters.WallJumpBoxAngle;
            _fallingVelocity = player.PlayerParameters.WallPressFallingVelocity;
            _jumpForce = player.PlayerParameters.WallJumpForce;
            _rigidbody = player.GetComponent<Rigidbody2D>();
            _playerAnimator = player.AnimatorController;
        }

        public bool HoldingWall { get; private set; }

        public bool IsTouchingWallRight { get; private set; }
        public bool IsTouchingWallLeft { get; private set; }
        public bool IsTouchingWall { get; private set; }

        public bool IsTouchingWallRightTest =>
            Physics2D.OverlapBoxNonAlloc((Vector2) _player.transform.position + _boxOffsetRight, _boxSizes, _boxAngle,
                _raycastRight, _layers) > 0;

        public bool IsTouchingWallLeftTest =>
            Physics2D.OverlapBoxNonAlloc((Vector2) _player.transform.position + _boxOffsetLeft, _boxSizes, _boxAngle,
                _raycastLeft, _layers) > 0;

        public void Tick()
        {
            if (_player.Grounder.IsGrounded) return;

            IsTouchingWallRight = IsTouchingWallRightTest;
            IsTouchingWallLeft = IsTouchingWallLeftTest;
            IsTouchingWall = IsTouchingWallRight || IsTouchingWallLeft;

            if (IsTouchingWallRight && RewiredPlayerInput.Instance.MovingRight
                || IsTouchingWallLeft && RewiredPlayerInput.Instance.MovingLeft)
            {
                Vector2 velocity = _rigidbody.velocity;
                _rigidbody.velocity =
                    new Vector2(velocity.x, Mathf.Clamp(velocity.y, -_fallingVelocity, float.MaxValue));
                HoldingWall = true;
            }
            else
            {
                HoldingWall = false;
            }

            if (RewiredPlayerInput.Instance.InitiateJump && IsTouchingWall)
            {
                if (IsTouchingWallRight)
                    _rigidbody.velocity = _jumpForce;
                else if (IsTouchingWallLeft) _rigidbody.velocity = _jumpForce.ReflectXAxis();

                HoldingWall = false;
                _player.CanMove = false;
                _player.StartCoroutine(WaitUntilTurnMoverBackOn());
            }

            _playerAnimator.HoldWall(HoldingWall);
        }

        public IEnumerator WaitUntilTurnMoverBackOn()
        {
            yield return new WaitForSeconds(_moverTimeOff);

            _player.CanMove = true;
        }
    }
}