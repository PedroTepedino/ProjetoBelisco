using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Collider2D))]
    public class Spikes : MonoBehaviour
    {
        [SerializeField]
        [ValidateInput("IsValuePositive", "The damage should be greater than Zero", InfoMessageType.Error)]
        private int _damage = 10;
        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<IHittable>()?.Hit(_damage);
        }

        private void OnValidate()
        {
            this.GetComponent<Collider2D>().isTrigger = true;
        }
        
    #if UNITY_EDITOR
        private static bool IsValuePositive(int value) => value > 0;
    #endif //UNITY_EDITOR
    }
}