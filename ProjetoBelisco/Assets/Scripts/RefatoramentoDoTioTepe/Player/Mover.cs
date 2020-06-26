using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Mover : IMover
    {
        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody2D;

        public Mover(Player player)
        {
            _player = player;
            _rigidbody2D = _player.GetComponent<Rigidbody2D>();
        }

        public void Tick()
        {
            float horizontalMovement = RewiredPlayerInput.Instance.Horizontal * _player.PlayerParameters.MovementSpeed;
            Vector2 movement = new Vector2( horizontalMovement, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = movement;
        }
    }
}