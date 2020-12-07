using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Belisco
{
    [CreateAssetMenu(fileName = "ExplosionAttack", menuName = "EnemyAttacks/ExplosionAttack")]
    public class ExplosionAttack : BaseAttack
    {
        [SerializeField] protected float attackRadius;

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
                Damage(attackDamage, hit);
            }
        }

    }
}