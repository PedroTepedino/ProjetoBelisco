using UnityEngine;

namespace Belisco
{
    public class GolemBossAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Animator animatorLeftBlade;
        [SerializeField] private Animator animatorRightBlade;

        protected EnemyStateMachine controller;
        protected IState currentState;
        protected StateMachine stateMachine;

        private void Awake()
        {
            controller = GetComponent<EnemyStateMachine>();
        }

        private void Update()
        {
            currentState = stateMachine.CurrentState;

            if (controller.alive)
            {
                animator.SetBool("Dead", false);
            }
            else
            {
                animator.SetBool("Dead", true);
            }
        }
        public void TriggerAnimationAttack(string animationName)
        {
            animator.SetTrigger(animationName);
        }

        public void TriggerAnimationLeftAttack(string animationName)
        {
            animatorLeftBlade.SetTrigger(animationName);
        }

        public void TriggerAnimationRightAttack(string animationName)
        {
            animatorRightBlade.SetTrigger(animationName);
        }

        public void TriggerAnimationHit()
        {
            animator.SetTrigger("Hit");
            animatorLeftBlade.SetTrigger("Hit");
            animatorRightBlade.SetTrigger("Hit");
        }

    }
}