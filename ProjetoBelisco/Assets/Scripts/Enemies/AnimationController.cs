using UnityEngine;

namespace Belisco
{
    public class AnimationController : MonoBehaviour
    {
        private Animator animator;
        private IEnemyStateMachine controller;

        private IState currentState;
        private StateMachine stateMachine;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            controller = GetComponent<IEnemyStateMachine>();
            stateMachine = controller.stateMachine;

            // _attack.OnAttack += ListenAttack;
            // _life.OnEnemyDamage += ListenDamage;
        }

        private void Update()
        {
            currentState = stateMachine.CurrentState;

            if (currentState is ChaseState)
            {
                animator.SetBool("Chase", true);
            }
            else
            {
                animator.SetBool("Chase", false);
            }

            if (currentState is AlertState)
            {
                animator.SetBool("Alert", true);
            }
            else
            {
                animator.SetBool("Alert", false);
            }

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

        private void OnDestroy()
        {
            // _attack.OnAttack -= ListenAttack;
            // _life.OnEnemyDamage -= ListenDamage;
        }

        public void TriggerAnimationAttack()
        {
            animator.SetTrigger("Attack");
        }

        public void TriggerAnimationHit()
        {
            animator.SetTrigger("Hit");
        }

        // private void ListenAttack(int index)
        // {
        //     _animator.SetInteger("AttackIndex", index);
        //     _animator.SetTrigger("Attack");
        // }

        // private void ListenDamage(int damage, int maxHealth)
        // {
        //     _animator.SetTrigger("Hit");
        // }

        public void Interfacinha()
        {
        }
    }
}