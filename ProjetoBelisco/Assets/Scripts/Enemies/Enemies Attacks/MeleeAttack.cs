using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Enemies
{
    public class MeleeAttack : IAttack
    {
        private void MeleeAttack()
        {
            Collider2D[] rayHits = Physics2D.OverlapCircleAll((Vector2)(this.transform.position) + (controller.movingRight ? attackPoint : -attackPoint), meleeAttackRadius, collisionLayerMask);
            Collider2D hit = CheckHit(rayHits);
            if (hit != null)
            {
                Debug.Log("melee");
                hit.gameObject.GetComponent<Player.Life>().Damage(meleeAttackDamage);
            }
        }
    }
}
