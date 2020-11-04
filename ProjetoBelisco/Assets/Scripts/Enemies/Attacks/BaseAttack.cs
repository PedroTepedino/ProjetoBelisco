using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Belisco
{

    [CreateAssetMenu(fileName = "BaseAttack", menuName = "EnemyAttacks/BaseAttack")]
    public abstract class BaseAttack : ScriptableObject
    {
        public int attackChance;
        public Vector2 attackCenter;
        public int attackDamage;
        public string attackAnimationName;
        public Transform ownerTransform;
        public LayerMask collisionLayerMask;
        public EnemyStateMachine controller;

        public abstract void AttackAction(Transform transform, LayerMask layerMask, EnemyStateMachine enemyStateMachine);

        protected Collider2D CheckHit(Collider2D[ ] hits)
        {
            foreach (Collider2D hit in hits)
                if (hit.CompareTag("Player"))
                {
                    RaycastHit2D raycastHit2D =
                        Physics2D.Linecast(ownerTransform.position, hit.transform.position, collisionLayerMask);
                    if (raycastHit2D.transform.gameObject == hit.gameObject)
                        return hit;
                    return null;
                }

            return null;
        }

        protected void Damage(int value, Collider2D hit)
        {
            IHittable hitable = hit.gameObject.GetComponent<IHittable>();
            if (hitable != null)hitable.Hit(value);
        }
    }
}