using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Belisco
{
    [CreateAssetMenu(fileName = "RangedAttack", menuName = "EnemyAttacks/RangedAttack")]
    public class RangedAttack : BaseAttack
    {
        [SerializeField] protected string attackProjectileTag;

        public override void AttackAction(Transform transform, LayerMask layerMask, EnemyStateMachine enemyStateMachine)
        {
            ownerTransform = transform;
            collisionLayerMask = layerMask;
            controller = enemyStateMachine;

            Pooler.Instance.SpawnFromPool(attackProjectileTag, (Vector2)transform.position + attackCenter,
                transform.rotation * Quaternion.Euler(0, 0, -90f));
        }

    }
}