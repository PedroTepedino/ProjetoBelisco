using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Belisco
{
    [CreateAssetMenu(fileName = "RadialShootAttack", menuName = "EnemyAttacks/RadialShootAttack")]
    public class RadialShootAttack : BaseAttack
    {
        [SerializeField] protected string attackProjectileTag;
        [SerializeField] protected int projectilesQuantity = 1;

        private float angle;

        public override void AttackAction(Transform transform, LayerMask layerMask, EnemyStateMachine enemyStateMachine)
        {
            ownerTransform = transform;
            collisionLayerMask = layerMask;
            controller = enemyStateMachine;

            angle = 360f / projectilesQuantity;

            for (int i = 0; i < projectilesQuantity; i++)
            {
                Pooler.Instance.SpawnFromPool(attackProjectileTag, (Vector2)transform.position + attackCenter, transform.rotation * Quaternion.Euler(0, 0, (-90f + (angle * i))));
            }

        }

    }
}