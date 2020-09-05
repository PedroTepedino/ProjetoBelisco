using Rewired.Platforms.PS4.Internal;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class BasicAttacker : IAttacker
    {
        private PlayerAnimatorController _animatorController;

        private readonly Player _player;
        private readonly float _timeBetweenAttacks;
        private float _timer;
        private Directions _attackDirection;
     
        public BasicAttacker(Player player)
        {
            _player = player;
            _animatorController = player.AnimatorController;
            _timeBetweenAttacks = player.PlayerParameters.TimeBetweenAttacks;
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
        }
    }
}