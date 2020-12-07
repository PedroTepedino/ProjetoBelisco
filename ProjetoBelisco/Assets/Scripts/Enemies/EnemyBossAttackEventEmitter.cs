using UnityEngine;

namespace Belisco
{
    public class EnemyBossAttackEventEmitter : MonoBehaviour
    {
        [SerializeField] private BossAttack attack;

        public void AttackFinished()
        {
            attack.ListenAttackFinished();
        }

        // public void MeleeAttack()
        // {
        //     attack.ListenAttackFinished(0);
        // }

        // public void ExplosionAttack()
        // {
        //     attack.ListenAttackFinished(1);
        // }

        // public void RangedAttack()
        // {
        //     attack.ListenAttackFinished(2);
        // }

        // public void DashAttack()
        // {
        //     attack.ListenAttackFinished(3);
        // }

        // public void ShootAndExplosion()
        // {
        //     attack.ListenAttackFinished(4);
        // }
    }
}