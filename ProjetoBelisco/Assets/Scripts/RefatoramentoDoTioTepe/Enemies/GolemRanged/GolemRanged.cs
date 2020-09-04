using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace RefatoramentoDoTioTepe
{
    public class GolemRanged : MonoBehaviour , IHittable, IEnemyStateMachine
    {
        private Rigidbody2D rigidbody;
        private Targeting targeting;
        private IAnimationController animationController;
        private Attack attack;
        private int healthPoints;

        public StateMachine stateMachine {get;set;}
        public bool movingRight {get;set;} = true;
        public bool alive {get;set;} = true;

        [SerializeField] [AssetsOnly] [InlineEditor] private EnemyParameters _enemyParameters;
        public EnemyParameters EnemyParameters => _enemyParameters;

        private void Awake() {
            animationController = this.GetComponentInChildren<IAnimationController>();
            rigidbody = this.GetComponent<Rigidbody2D>();
            targeting = this.GetComponent<Targeting>();
            attack = this.GetComponent<Attack>();
            healthPoints = EnemyParameters.MaxHealthPoints;

            stateMachine = new StateMachine();

            var moveState = new MoveState(this.gameObject);
            var iddleState = new IddleState(this.gameObject);
            var chaseState = new ChaseState(this.gameObject);
            var attackState = new AttackState(this.gameObject);
            var alertState = new AlertState(this.gameObject);

            stateMachine.SetState(iddleState);

            stateMachine.AddTransition(iddleState, moveState, iddleState.TimeEnded);
            stateMachine.AddTransition(iddleState, alertState, iddleState.TimeEnded);

            stateMachine.AddTransition(moveState, chaseState, () => targeting.hasTarget);
            stateMachine.AddTransition(moveState, alertState, () => targeting.hasTarget);       

            stateMachine.AddTransition(chaseState, attackState, () => attack.isInRange);
            stateMachine.AddTransition(chaseState, moveState, () => !targeting.hasTarget);

            stateMachine.AddTransition(attackState, moveState, () => !targeting.hasTarget);
            stateMachine.AddTransition(attackState, chaseState, () => (!attack.isInRange && targeting.hasTarget));

            stateMachine.AddTransition(alertState, moveState, () => !targeting.hasTarget);
            stateMachine.AddTransition(alertState, chaseState, () => (alertState.TimeEnded() && targeting.hasTarget && !attack.isInRange));
            stateMachine.AddTransition(alertState, attackState, () => (alertState.TimeEnded() && targeting.hasTarget && attack.isInRange));
        }

        private void Update() {
            stateMachine.Tick();
        }

        public void Hit(int damage)
        {
            if (healthPoints <= 0)
            {
                this.gameObject.SetActive(false);
                alive = false;
            }else
            {
                animationController.TriggerAnimationHit();
            }

        }

        public void Interfacinha(){}

    }
}
