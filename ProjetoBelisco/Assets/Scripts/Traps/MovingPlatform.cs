using UnityEngine;

namespace Belisco
{
    [RequireComponent(typeof(Collider2D))]
    public class MovingPlatform : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player")) other.transform.parent = transform;
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.transform.parent == transform) other.transform.parent = null;
        }
    }
}