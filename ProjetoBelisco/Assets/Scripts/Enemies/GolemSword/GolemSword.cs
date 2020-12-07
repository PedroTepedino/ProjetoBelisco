using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    public class GolemSword : EnemyStateMachine
    {

        protected override void Awake()
        {
            base.Awake();

            MoveState moveState = new MoveState(gameObject);
            IddleState iddleState = new IddleState(gameObject);
            ChaseState chaseState = new ChaseState(gameObject);
            AttackState attackState = new AttackState(gameObject);
            AlertState alertState = new AlertState(gameObject);

            stateMachine.SetState(iddleState);

            stateMachine.AddTransition(iddleState, moveState, () => iddleState.TimeEnded() && !targeting.hasTarget);
            stateMachine.AddTransition(iddleState, alertState, () => iddleState.TimeEnded() && targeting.hasTarget);

            stateMachine.AddTransition(moveState, iddleState, moveState.TimeEnded);
            stateMachine.AddTransition(moveState, alertState, () => targeting.hasTarget);

            stateMachine.AddTransition(chaseState, attackState, () => attack.isInRange);
            stateMachine.AddTransition(chaseState, iddleState, () => !targeting.hasTarget);

            stateMachine.AddTransition(attackState, iddleState, () => !targeting.hasTarget);
            stateMachine.AddTransition(attackState, chaseState, () => !attack.isInRange && targeting.hasTarget);

            stateMachine.AddTransition(alertState, iddleState, () => !targeting.hasTarget);
            stateMachine.AddTransition(alertState, chaseState,
                () => alertState.TimeEnded() && targeting.hasTarget && !attack.isInRange);
            stateMachine.AddTransition(alertState, attackState,
                () => alertState.TimeEnded() && targeting.hasTarget && attack.isInRange);
        }
    }
}