using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    public class GolemSimple : EnemyStateMachine
    {

        protected override void Awake()
        {
            base.Awake();

            MoveState moveState = new MoveState(gameObject);
            IddleState iddleState = new IddleState(gameObject);
            ChaseState chaseState = new ChaseState(gameObject);
            AlertState alertState = new AlertState(gameObject);

            stateMachine.SetState(iddleState);

            stateMachine.AddTransition(iddleState, moveState, () => iddleState.TimeEnded() && !targeting.hasTarget);
            stateMachine.AddTransition(iddleState, alertState, () => iddleState.TimeEnded() && targeting.hasTarget);

            stateMachine.AddTransition(moveState, iddleState, moveState.TimeEnded);
            stateMachine.AddTransition(moveState, alertState, () => targeting.hasTarget);

            stateMachine.AddTransition(chaseState, iddleState, () => !targeting.hasTarget);

            stateMachine.AddTransition(alertState, iddleState, () => !targeting.hasTarget);
            stateMachine.AddTransition(alertState, chaseState, () => alertState.TimeEnded() && targeting.hasTarget);
        }
    }
}