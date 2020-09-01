using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class StrongAttacker : IAttacker
    {
        public Vector2 _position;

        public StrongAttacker(Player player)
        {
            _position = player.PlayerParameters.StrongAttackCenter;
        }

        public void Tick()
        {
            if (RewiredPlayerInput.Instance.StrongAttack)
            {
                
            }
        }

        public void Attack()
        {
            
        }
    }
}