using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameScripts.Player
{
    public class PlatformDown : MonoBehaviour
    {
        [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _grounderCenter = Vector3.zero;
        [FoldoutGroup("Parameters")] [SerializeField] private Vector2 _grounderSizes;
        [FoldoutGroup("Parameters")] [SerializeField] [EnumToggleButtons] private LayerMask _grounderLayerMask;

        private PlatformEffector2D _effector;
        private Collider2D _collider;

        public static bool IsFallingThrough { get; private set; } = false;

        public static Action OnDownfall;

        public bool GetDownFromPlatform()
        {
            Collider2D collider = Physics2D.OverlapBox(this.transform.position + _grounderCenter, _grounderSizes, 0f, _grounderLayerMask);

            if (collider != null)
            {
                _effector = collider.gameObject.GetComponentInChildren<PlatformEffector2D>();
                if (_effector != null)
                {
                    _effector.rotationalOffset = 180f;
                
                    _collider = collider.GetComponentInChildren<Collider2D>();
                
                    if (_collider.usedByComposite)
                    {
                        _collider = collider.GetComponentInChildren<CompositeCollider2D>();
                        if (_collider == null)
                        {
                            _collider = collider.GetComponentInParent<CompositeCollider2D>();
                        }
                    }

                    _collider.isTrigger = true;
                
                    OnDownfall?.Invoke();

                    IsFallingThrough = true;
            
                    return true;
                }

                return false;
            }
        
            IsFallingThrough = false;
            return false;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_effector == null) return;
        
            if (_effector.gameObject != other.gameObject) return;
        
            _effector.rotationalOffset = 0f;

            _collider.isTrigger = false;

            _effector = null;
            _collider = null;

            IsFallingThrough = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            IsFallingThrough = false;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            IsFallingThrough = false;
        }


        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(this.transform.position + _grounderCenter, _grounderSizes);
        }
    }
}
