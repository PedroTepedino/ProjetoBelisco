using System;
using UnityEngine;

namespace Belisco
{
    public class Mover : IMover
    {
        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody2D;
        private readonly float _walkSpeed;

        public Mover(Player player)
        {
            _player = player;
            _rigidbody2D = _player.GetComponent<Rigidbody2D>();
            _walkSpeed = _player.PlayerParameters.MovementSpeed;
        }

        public void Tick()
        {
            float processedInput = 0;
            if (RewiredPlayerInput.Instance.Horizontal > 0.30f || RewiredPlayerInput.Instance.Horizontal < -0.30f)
                processedInput = RewiredPlayerInput.Instance.Horizontal;

            var horizontalMovement = processedInput * _walkSpeed;
            Vector2 movement = new Vector2(horizontalMovement, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = movement;
        }
    }

    public class Dasher : IMover
    {
        private readonly float _dashDirectionMultiplier;
        private readonly float _dashVelocity;
        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody2D;

        private readonly float _timeToEndAnimation = 0.2f;
        private float _damping;
        private float _timer;

        public Dasher(Player player)
        {
            Dashing = true;
            _player = player;
            _rigidbody2D = _player.GetComponent<Rigidbody2D>();
            _dashDirectionMultiplier = _player.GetDirectionHorizontal() == Directions.Left ? -1f : 1f;
            _dashVelocity = _player.PlayerParameters.DashVelocity;
        }

        public bool Dashing { get; private set; }

        public void Tick()
        {
            if (Dashing)
            {
                _rigidbody2D.velocity = new Vector2(_dashDirectionMultiplier * _dashVelocity, 0f);
            }
            else
            {
                _timer += Time.deltaTime;
                var xVelocity = _dashVelocity * (1 - _timer / _timeToEndAnimation);
                _rigidbody2D.velocity = new Vector2(_dashDirectionMultiplier * (xVelocity < 0 ? 0 : xVelocity),
                    _rigidbody2D.velocity.y);
            }
        }

        public static bool CheckDashInput()
        {
            return RewiredPlayerInput.Instance.Dash;
        }

        public void StopDashing()
        {
            Dashing = false;
        }
    }

    public class NewMover : IMover
    {
        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody2D;
        private float _walkAcceleration;
        private float _walkSpeed;

        public NewMover(Player player)
        {
            _player = player;
            _rigidbody2D = _player.GetComponent<Rigidbody2D>();
            _walkSpeed = _player.PlayerParameters.MovementSpeed;
            _walkAcceleration = _player.PlayerParameters.WalkAcceleration;
        }

        public void Tick()
        {
            _walkSpeed = _player.PlayerParameters.MovementSpeed;
            _walkAcceleration = _player.PlayerParameters.WalkAcceleration;

            var inputValue = RewiredPlayerInput.Instance.HorizontalInt;
            if (_player.Grounder.IsGrounded)
            {
                if (Mathf.Abs(inputValue) > 0.30f)
                {
                    if (Math.Abs(Mathf.Abs(_rigidbody2D.velocity.x) - _walkSpeed) < 0.2f)
                    {
                        var horizontalMovement = inputValue * _walkSpeed;
                        Vector2 movement = new Vector2(horizontalMovement, _rigidbody2D.velocity.y);
                        _rigidbody2D.velocity = movement;
                    }
                    else
                    {
                        var horizontalMovement = Mathf.Lerp(_rigidbody2D.velocity.x, inputValue * _walkSpeed,
                            Time.deltaTime * _walkAcceleration);
                        Vector2 movement = new Vector2(horizontalMovement, _rigidbody2D.velocity.y);
                        _rigidbody2D.velocity = movement;
                    }
                }
                else
                {
                    var horizontalMovement =
                        Mathf.Lerp(_rigidbody2D.velocity.x, 0f, Time.deltaTime * _walkAcceleration);
                    _rigidbody2D.velocity = new Vector2(horizontalMovement, _rigidbody2D.velocity.y);
                }
            }
            else
            {
                if (Mathf.Abs(inputValue) > 0.1f)
                {
                    var horizontalMovement = inputValue * _walkSpeed;
                    Vector2 movement = new Vector2(horizontalMovement, _rigidbody2D.velocity.y);
                    _rigidbody2D.velocity = movement;
                }
            }
        }
    }

    public class AutoMover : IMover
    {
        public void Tick()
        {
        }
    }

    public class ForceMover : IMover
    {
        private readonly Player _player;
        private Rigidbody2D _rigidbody;
        private readonly float _minVelocity;
        private readonly float _baseDrag;

        public ForceMover(Player player, Vector3 force)
        {
            _player = player;
            _rigidbody = _player.Rigidbody;
            _minVelocity = _player.PlayerParameters.MinPushVelocity;
            _baseDrag = _player.PlayerParameters.BaseDrag;

            _rigidbody.AddForce(force, ForceMode2D.Impulse);
            _rigidbody.drag = _player.PlayerParameters.KnockBackDrag;
        }

        public ForceMover(Player player, Vector3 force, float customDrag)
        {
            _player = player;
            _rigidbody = _player.Rigidbody;
            _minVelocity = _player.PlayerParameters.MinPushVelocity;
            _baseDrag = _player.PlayerParameters.BaseDrag;

            _rigidbody.AddForce(force, ForceMode2D.Impulse);
            _rigidbody.drag = customDrag;
        }

        public void Tick()
        {
            if (_rigidbody.velocity.magnitude <= _minVelocity)
            {
                _rigidbody.drag = _baseDrag;
                _player.ReturnToBaseMover();
            }
        }
    }
}