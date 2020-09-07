using System.Collections.Generic;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class BasicAttacker : IAttacker
    {
        private PlayerAnimatorController _animatorController;

        private readonly Player _player;
        private Directions _attackDirection;
        private readonly Dictionary<Directions, AttackParameter> _attackParameters;
        private readonly int _attackDamage;
        private readonly LayerMask _attackLayers;

        public BasicAttacker(Player player)
        {
            _player = player;
            _animatorController = player.AnimatorController;
            _attackDamage = _player.PlayerParameters.BasicAttackDamage;
            _attackLayers = _player.PlayerParameters.AttackLayers;
            _attackParameters = new Dictionary<Directions, AttackParameter>()
            {
                {Directions.Right, _player.PlayerParameters.BasicRightAttackParameter},
                {Directions.Left, _player.PlayerParameters.BasicLeftAttackParameter},
                {Directions.Up, _player.PlayerParameters.BasicUpAttackParameter},
                {Directions.Down, _player.PlayerParameters.BasicDownAttackParameter},
            };
        }

        public void Tick()
        {
            if (RewiredPlayerInput.Instance.Attack && AttackerTimer.TimerEnded)
            {
                _attackDirection = _player.GetAttackDirection();
                _animatorController.Attack(_attackDirection);
                _player.StartAttack();
            }
        }

        public void Attack()
        {
            Debug.Log($"Attack {_attackDirection}");

            Collider2D[] collisions = GetCollisions(_attackParameters[_attackDirection]);

            foreach (var collider in collisions)
            {
                var hittable = collider.gameObject.GetComponent<IHittable>();
                if (hittable != null)
                {
                    if (!HasWallBetween(collider.transform.position))
                    {
                        hittable.Hit(_attackDamage);
                    }
                }
            }
        }

        private bool HasWallBetween(Vector2 position)
        {
            var playerPosition = (Vector2) (_player.transform.position);
            var direction = position - playerPosition;
            var distance = direction.magnitude;
            direction = direction.normalized;

            return Physics2D.Raycast(playerPosition, direction, distance, LayerMask.GetMask("Ground")).collider != null;
        }

        private Collider2D[] GetCollisions(AttackParameter parameter) => Physics2D.OverlapCircleAll((Vector2)(_player.transform.position) + parameter.Center, parameter.Radius, _attackLayers);
    }

    public readonly struct AttackParameter
    {
        public readonly Vector2 Center;
        public readonly float Radius;

        public AttackParameter(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }
    }
}