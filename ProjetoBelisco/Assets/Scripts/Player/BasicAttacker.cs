using System.Collections.Generic;
using UnityEngine;

namespace Belisco
{
    public class BasicAttacker : IAttacker
    {
        private readonly int _attackDamage;
        private readonly LayerMask _attackLayers;
        private readonly Dictionary<Directions, AttackParameter> _attackParameters;

        private readonly Player _player;
        private readonly PlayerAnimatorController _animatorController;
        private Directions _attackDirection;

        public BasicAttacker(Player player)
        {
            _player = player;
            _animatorController = player.AnimatorController;
            _attackDamage = _player.PlayerParameters.BasicAttackDamage;
            _attackLayers = _player.PlayerParameters.AttackLayers;
            _attackParameters = new Dictionary<Directions, AttackParameter>
            {
                {Directions.Right, _player.PlayerParameters.BasicRightAttackParameter},
                {Directions.Left, _player.PlayerParameters.BasicLeftAttackParameter},
                {Directions.Up, _player.PlayerParameters.BasicUpAttackParameter},
                {Directions.Down, _player.PlayerParameters.BasicDownAttackParameter}
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
            var collisions = GetCollisions(_attackParameters[_attackDirection]);

            foreach (Collider2D collider in collisions)
            {
                IHittable hittable = collider.gameObject.GetComponent<IHittable>();
                if (hittable != null)
                {
                    if (!HasWallBetween(collider.transform.position))
                    {
                        hittable.Hit(_attackDamage, _player.transform);
                        Debug.Log($"Hit {collider.gameObject}", collider.transform);
                    }
                    else if (hittable is Switch)
                    {
                        hittable.Hit(_attackDamage, _player.transform);
                    }
                }
            }
        }

        private bool HasWallBetween(Vector2 position)
        {
            Vector2 playerPosition = (Vector2) _player.transform.position;
            Vector2 direction = position - playerPosition;
            var distance = direction.magnitude;
            direction = direction.normalized;

            return Physics2D.Raycast(playerPosition, direction, distance, LayerMask.GetMask("Ground")).collider != null;
        }

        private Collider2D[] GetCollisions(AttackParameter parameter)
        {
            return Physics2D.OverlapCircleAll((Vector2) _player.transform.position + parameter.Center, parameter.Radius,
                _attackLayers);
        }
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