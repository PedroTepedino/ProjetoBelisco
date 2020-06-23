using System;
using System.Collections;
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Collider2D))]
    public class Paw : MonoBehaviour
    {
        [SerializeField] private int _value = 1;
        public int Value => _value;

        public void InitializeValue(int valeu = 0)
        {
            if (Value > 0)
            {
                _value = valeu;
            }
            else
            {
                _value = 1;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var inventory = other.GetComponent<PawInventory>();
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
            
            var layer = LayerMask.NameToLayer("Collectables");
            if (this.gameObject.layer != layer)
            {
                gameObject.layer = layer;
            }
        }
    }
}