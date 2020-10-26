using UnityEngine;

namespace Belisco
{
    public class StrongAttacker : IAttacker
    {
        private readonly Player _player;
        private readonly PlayerAnimatorController _animatorController;
        private Directions _attackDirection;


        public StrongAttacker(Player player)
        {
            _player = player;
            _animatorController = player.AnimatorController;
        }

        public void Tick()
        {
            if (RewiredPlayerInput.Instance.StrongAttack && AttackerTimer.TimerEnded)
            {
                _attackDirection = _player.GetDirectionHorizontal();
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