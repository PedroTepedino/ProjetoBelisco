using UnityEngine;

namespace Belisco
{
    public class GolemTripleAnimation : EnemyAnimationController
    {
        protected override void Update()
        {
            currentState = stateMachine.CurrentState;

            if (currentState is ChaseState)
                animator.SetBool("Chase", true);
            else
                animator.SetBool("Chase", false);

            if (currentState is AlertState)
                animator.SetBool("Alert", true);
            else
                animator.SetBool("Alert", false);

            if (controller.alive)
            {
                animator.SetBool("Dead", false);
            }
            else
            {
                animator.SetBool("Dead", true);
                animator.SetBool("Alert", false);
                animator.SetBool("Chase", false);
            }
        }
    }
}