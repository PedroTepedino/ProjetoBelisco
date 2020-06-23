using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Collider2D))]
    public class Pawn : MonoBehaviour
    {
        [SerializeField] private int _value = 1;
        public int Value => _value;

        private void OnTriggerEnter(Collider other)
        {
            var inventory = other.GetComponent<PawnInventory>();
            if (inventory != null)
            {
                inventory.PickUp(this);
            }
        }

        private void OnValidate()
        {
            var colliders = GetComponents<Collider2D>();

            bool hasOneTrigger = false;
            foreach (var coll in colliders)
            {
                if (coll.isTrigger)
                {
                    hasOneTrigger = true;
                }
            }

            if (hasOneTrigger == false)
            {
                colliders[colliders.Length - 1].isTrigger = true;
            }

            if (_value <= 0)
            {
                _value = 1;
            }
        }
    }
}