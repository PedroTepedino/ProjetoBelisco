using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class StrongAttacker : IAttacker
    {
        private PlayerAnimatorController _animatorController;
        private readonly Player _player;
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