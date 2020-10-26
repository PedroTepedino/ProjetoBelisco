using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    [RequireComponent(typeof(Collider2D))]
    public class Spikes : MonoBehaviour
    {
        [SerializeField] [ValidateInput("IsValuePositive", "The damage should be greater than Zero")]
        private int _damage = 10;

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<IHittable>()?.Hit(_damage);
        }

        private void OnValidate()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

#if UNITY_EDITOR
        private static bool IsValuePositive(int value)
        {
            return value > 0;
        }
#endif //UNITY_EDITOR
    }
}