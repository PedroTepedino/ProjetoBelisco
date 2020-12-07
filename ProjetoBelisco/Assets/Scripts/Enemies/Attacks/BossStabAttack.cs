using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Belisco
{
    [CreateAssetMenu(fileName = "BossStabAttack", menuName = "EnemyAttacks/BossStabAttack")]
    public class BossStabAttack : BaseAttack
    {
        [SerializeField] protected float attackRadius;
        [SerializeField] protected float stabSpeed;
        [SerializeField] protected float stabDistance;
        [SerializeField] protected bool weaponRight;
        [SerializeField] protected BossBlades weapon;

        public override void AttackAction(Transform transform, LayerMask layerMask, EnemyStateMachine enemyStateMachine)
        {
            ownerTransform = transform;
            collisionLayerMask = layerMask;
            controller = enemyStateMachine;

            weapon.stabSpeed = stabSpeed;
            weapon.stabDistance = stabDistance;
            weapon.weaponRight = weaponRight;
            weapon.weaponInitialPosition = weapon.transform.position;
            weapon.damage = attackDamage;
            weapon.isAttacking = true;

        }

    }
}