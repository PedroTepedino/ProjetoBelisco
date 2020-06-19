using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Jumper : IJumper
    {
        private Rigidbody2D _rigidbody;
        private Grounder _playerGrounder;

        public Jumper(Player player)
        {
            _rigidbody = player.gameObject.GetComponent<Rigidbody2D>();
            _playerGrounder = player.Grounder;
        }

        public void Tick()
        {
            Vector2 velocity = _rigidbody.velocity;

            if (RewiredPlayerInput.Instance.Jump && _playerGrounder.IsGrounded)
                velocity.y = 1;

            _rigidbody.velocity = velocity;
        }
    }
}