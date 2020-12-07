using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    public class GolemBoss : EnemyStateMachine
    {

        protected override void Awake()
        {
            base.Awake();

            IddleState iddleState = new IddleState(gameObject);
            AttackState attackState = new AttackState(gameObject);

            stateMachine.SetState(iddleState);

            stateMachine.AddTransition(iddleState, attackState, () => targeting.hasTarget);

            stateMachine.AddTransition(attackState, iddleState, () => !targeting.hasTarget);
        }
    }
}