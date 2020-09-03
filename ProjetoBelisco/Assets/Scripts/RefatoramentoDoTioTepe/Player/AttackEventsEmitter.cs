using System;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class AttackEventsEmitter : MonoBehaviour
    {
        [SerializeField] private Player _player;
        
        public void StrongAttack()
        {
            _player.CallAttack<StrongAttacker>();
        }

        public void BasicAttack()
        {
            _player.CallAttack<BasicAttacker>();
        }

        public void EndAttack()
        {
            _player.EndAttack();
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