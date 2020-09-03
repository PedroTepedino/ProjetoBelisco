using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class AnimationController : MonoBehaviour
    {
        private Animator animator;
        private EnemyStateMachine controller;
        private StateMachine stateMachine;

        private IState currentState;

        private void Awake()
        {
            animator = this.GetComponentInChildren<Animator>();
            controller = this.GetComponent<EnemyStateMachine>();
            stateMachine = controller.stateMachine;

            // _attack.OnAttack += ListenAttack;
            // _life.OnEnemyDamage += ListenDamage;
        }

        private void OnDestroy()
        {
            // _attack.OnAttack -= ListenAttack;
            // _life.OnEnemyDamage -= ListenDamage;
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
    }
}
