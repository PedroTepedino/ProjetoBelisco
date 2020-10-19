using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Collider2D))]
    public class MovingPlatform : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.transform.parent = this.transform;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.transform.parent == this.transform)
            {
                other.transform.parent = null;
            }
        }
    }
}