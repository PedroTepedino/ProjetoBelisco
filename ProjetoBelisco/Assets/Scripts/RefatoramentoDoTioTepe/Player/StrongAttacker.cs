﻿using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class StrongAttacker : IAttacker
    {
        public readonly Vector2 _position;

        private PlayerAnimatorController _animatorController;

        private readonly Player _player;
        private Directions _attackDirection;

        public StrongAttacker(Player player)
        {
            _player = player;
            _position = player.PlayerParameters.StrongAttackCenter;
            _animatorController = player.AnimatorController;
        }

        public void Tick()
        {
            if (RewiredPlayerInput.Instance.StrongAttack)
            {
                _attackDirection = _player.GetAttackDirectionHorizontal();
                _animatorController.StrongAttack();
                _player.StartAttack();
            }
        }

        public void Attack()
        {
            Debug.Log($"Attack {_attackDirection}");
        }
    }
}