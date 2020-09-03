using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class BasicAttacker : IAttacker
    {
        private PlayerAnimatorController _animatorController;

        private readonly Player _player;
        private Directions _attackDirection;
     
        public BasicAttacker(Player player)
        {
            _player = player;
            _animatorController = player.AnimatorController;
        }

        public void Tick()
        {
            if (RewiredPlayerInput.Instance.Attack)
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