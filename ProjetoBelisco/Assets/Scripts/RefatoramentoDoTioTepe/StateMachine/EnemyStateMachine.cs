using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace RefatoramentoDoTioTepe
{
    public class EnemyStateMachine : MonoBehaviour
    {
        [SerializeField] [AssetsOnly] [InlineEditor] private EnemyParameters _enemyParameters;
        public EnemyParameters EnemyParameters => _enemyParameters;

        private StateMachine stateMachine;
        public Rigidbody2D rigidbody;
        public Targeting targeting;
        public Attack attack;
        public bool movingRight = true;

        private void Awake() {
            rigidbody = GetComponent<Rigidbody2D>();
            targeting = GetComponent<Targeting>();
            attack = GetComponent<Attack>();

            stateMachine = new StateMachine();

            var moveState = new MoveState(this.gameObject);
            var iddleState = new IddleState(this.gameObject);
            var chaseState = new ChaseState(this.gameObject);
            var attackState = new AttackState(this.gameObject);

            stateMachine.SetState(iddleState);

            stateMachine.AddTransition(iddleState, moveState, iddleState.TimeEnded);

            stateMachine.AddTransition(moveState, chaseState, () => targeting.hasTarget);      

            stateMachine.AddTransition(chaseState, attackState, () => attack.isInRange);
            stateMachine.AddTransition(chaseState, moveState, () => !targeting.hasTarget);

            stateMachine.AddTransition(attackState, moveState, () => !targeting.hasTarget);
            stateMachine.AddTransition(attackState, chaseState, () => (!attack.isInRange && targeting.hasTarget));
        }

        private void Update() {
            stateMachine.Tick();
        }

    }
}
