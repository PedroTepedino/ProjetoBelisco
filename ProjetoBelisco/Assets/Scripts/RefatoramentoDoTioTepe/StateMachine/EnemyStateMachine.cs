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

            var move = new MoveState(this.gameObject);
            var iddle = new IddleState(this.gameObject);
            var chase = new ChaseState(this.gameObject);
            var attack = new AttackState(this.gameObject);

            stateMachine.SetState(iddle);

            stateMachine.AddTransition(iddle, move, iddle.TimeEnded);

            stateMachine.AddTransition(move, chase, () => targeting.hasTarget);      

            stateMachine.AddTransition(chase, attack, () => attack.isInRange);
            stateMachine.AddTransition(chase, move, () => !targeting.hasTarget);

            stateMachine.AddTransition(attack, move, () => !targeting.hasTarget);
            stateMachine.AddTransition(attack, chase, () => (!attack.isInRange && targeting.hasTarget));
        }

        private void Update() {
            _stateMachine.Tick();
        }

    }
}
