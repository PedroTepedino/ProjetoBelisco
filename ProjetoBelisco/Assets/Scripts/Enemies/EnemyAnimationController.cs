using UnityEngine;

namespace Belisco
{
    public abstract class EnemyAnimationController : MonoBehaviour
    {
        protected Animator animator;
        protected EnemyStateMachine controller;

        protected IState currentState;
        protected StateMachine stateMachine;

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            controller = GetComponent<EnemyStateMachine>();
        }

        protected virtual void Start()
        {
            stateMachine = controller.stateMachine;
        }

        protected abstract void Update();

        public void TriggerAnimationAttack()
        {
            animator.SetTrigger("Attack");
        }

        public void TriggerAnimationHit()
        {
            animator.SetTrigger("Hit");
        }

    }
}