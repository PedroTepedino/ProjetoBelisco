using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Collider2D))]
    public class VoidHole : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Kill(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            Kill(collision);
        }

        private void Kill(Collider2D collision)
        {
            if (!collision.gameObject.activeInHierarchy) return;

            var player = collision.gameObject.GetComponent<Player>();
            if (player)
            {
                player.Hit(10000);
            }
        }
    }
}