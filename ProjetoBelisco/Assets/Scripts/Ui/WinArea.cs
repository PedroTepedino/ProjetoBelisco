using UnityEngine;

namespace Belisco
{
    [RequireComponent(typeof(Collider2D))]
    public class WinArea : MonoBehaviour
    {
        public static bool HasWon { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Player>())
                HasWon = true;
        }
    }
}