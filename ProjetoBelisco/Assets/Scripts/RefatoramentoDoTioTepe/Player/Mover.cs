﻿using UnityEngine;

namespace RefatoramentoDoTioTepe
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
            {
                processedInput = RewiredPlayerInput.Instance.Horizontal;
            }

            float horizontalMovement = processedInput * _walkSpeed;
            Vector2 movement = new Vector2( horizontalMovement, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = movement;
        }
    }

    public class Dasher : IMover
    {
        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody2D;
        private readonly float _dashVelocity;
        private bool _dashing;
        private float _damping;

        private readonly float _timeToEndAnimation = 0.2f;
        private float _timer;
        private readonly float _dashDirectionMultiplier;

        public Dasher(Player player)
        {
            _dashing = true;
            _player = player;
            _rigidbody2D = _player.GetComponent<Rigidbody2D>();
            _dashDirectionMultiplier = _player.GetDirectionHorizontal() == Directions.Left ? -1f : 1f;
            _dashVelocity = _player.PlayerParameters.DashVelocity;
        }

        public static bool CheckDashInput() => RewiredPlayerInput.Instance.Dash;

        public void Tick()
        {
            if (_dashing)
            {
                _rigidbody2D.velocity = new Vector2(_dashDirectionMultiplier *_dashVelocity, 0f);
            }
            else
            {
                _timer += Time.deltaTime;
                var xVelocity = _dashVelocity * (1 - (_timer / _timeToEndAnimation));
                _rigidbody2D.velocity = new Vector2(_dashDirectionMultiplier * (xVelocity < 0 ? 0 : xVelocity), _rigidbody2D.velocity.y);
            }
        }

        public void StopDashing()
        {
            _dashing = false;
            
        }
    }
}