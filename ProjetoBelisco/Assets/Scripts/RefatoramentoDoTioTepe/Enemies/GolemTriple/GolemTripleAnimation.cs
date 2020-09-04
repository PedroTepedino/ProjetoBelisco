using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class GolemTripleAnimation : MonoBehaviour
    {
        private Animator animator;
        private IEnemyStateMachine controller;
        private StateMachine stateMachine;

        private IState currentState;

        private void Awake()
        {
            animator = this.GetComponentInChildren<Animator>();
            controller = this.GetComponent<IEnemyStateMachine>();
            stateMachine = controller.stateMachine;
        }

        private void Update()
        {
            currentState = stateMachine.CurrentState;

            if (currentState is ChaseState)
            {
                animator.SetBool("Chase", true);
            }else
            {
                animator.SetBool("Chase", false);
            }

            if (currentState is AlertState)
            {
                animator.SetBool("Alert", true);
            }else
            {
                animator.SetBool("Alert", false);
            }

            if (controller.alive)
            {
                animator.SetBool("Dead", false);
            }else
            {
                animator.SetBool("Dead", true);
                animator.SetBool("Alert", false);
                animator.SetBool("Chase", false);
            }
        }

        public void TriggerAnimationHit()
        {
            animator.SetTrigger("Hit");
        }

        public void Interfacinha(){}

    }
}
