using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Belisco
{
    [CreateAssetMenu(fileName = "MeleeAttack", menuName = "EnemyAttacks/MeleeAttack")]
    public class MeleeAttack : BaseAttack
    {
        protected float attackRadius;

        public override void AttackAction(Transform transform, LayerMask layerMask, EnemyStateMachine enemyStateMachine)
        {
            ownerTransform = transform;
            collisionLayerMask = layerMask;
            controller = enemyStateMachine;

            var rayHits = Physics2D.OverlapCircleAll(
                (Vector2)transform.position + (controller.movingRight ? attackCenter : -attackCenter),
                attackRadius, collisionLayerMask);
            Collider2D hit = CheckHit(rayHits);
            if (hit != null)
            {
                Debug.Log("melee");
                Damage(attackDamage, hit);
            }
        }

    }
}