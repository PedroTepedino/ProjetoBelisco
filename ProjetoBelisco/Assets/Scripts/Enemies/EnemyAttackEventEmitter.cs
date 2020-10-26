using UnityEngine;

namespace Belisco
{
    public class EnemyAttackEventEmitter : MonoBehaviour
    {
        [SerializeField] private Attack attack;

        public void MeleeAttack()
        {
            attack.ListenAttackFinished(0);
        }

        public void ExplosionAttack()
        {
            attack.ListenAttackFinished(1);
        }

        public void RangedAttack()
        {
            attack.ListenAttackFinished(2);
        }

        public void DashAttack()
        {
            attack.ListenAttackFinished(3);
        }

        public void ShootAndExplosion()
        {
            attack.ListenAttackFinished(4);
        }
    }
}