using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Collider2D))]
    public class Spikes : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<IHittable>()?.Hit(10000);
        }
    }
}