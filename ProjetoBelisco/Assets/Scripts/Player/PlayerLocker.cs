using UnityEngine;

namespace Belisco
{
    public class PlayerLocker
    {
        private readonly Rigidbody2D _rigidbody;

        public PlayerLocker(Player player)
        {
            _rigidbody = player.GetComponent<Rigidbody2D>();
        }

        public void LockPlayer()
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void UnlockPlayer()
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody.AddForce(Vector2.one);
        }
    }
}