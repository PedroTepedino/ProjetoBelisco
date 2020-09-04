using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Glider
    {
        private Rigidbody2D _rigidbody;

        public Glider(Player player)
        {
            _rigidbody = player.GetComponent<Rigidbody2D>();
        }
        
        
    }
}