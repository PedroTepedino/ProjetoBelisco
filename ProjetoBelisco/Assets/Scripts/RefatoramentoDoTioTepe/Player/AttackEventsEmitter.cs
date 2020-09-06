using System;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class AttackEventsEmitter : MonoBehaviour
    {
        [SerializeField] private Player _player;
        
        public void StrongAttack()
        {
            _player.CallAttack();
        }

        public void BasicAttack()
        {
            _player.CallAttack();
        }

        public void EndAttack()
        {
            _player.EndAttack();
        }

        public void StopDashing()
        {
            _player.StopDashing();
        }

        public void EndDash()
        {
            _player.EndDash();
        }

        private void OnValidate()
        {
            if (_player == null)
            {
                _player = this.GetComponentInParent<Player>();
            }
        }
    }
}