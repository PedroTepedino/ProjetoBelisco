using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class WallJumper 
    {
        private readonly Player _player;
        private readonly Rigidbody _rigidbody;
        private readonly Grounder _grounder;
        
        public bool HoldingWall { get; private set; }
        
        public WallJumper(Player player)
        {
            _player = player;
            _rigidbody = _player.GetComponent<Rigidbody>();
            _grounder = _player.Grounder;
            HoldingWall = false;
        }
        
        public void Tick()
        {
            if (!_grounder.IsGrounded && HoldingWall)
            {
                
            }
        }
    }
}