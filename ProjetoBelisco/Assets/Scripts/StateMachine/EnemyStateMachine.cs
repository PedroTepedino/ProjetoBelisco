using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    public class EnemyStateMachine : MonoBehaviour, IHittable, IEnemyStateMachine
    {
        [SerializeField] [AssetsOnly] [InlineEditor]
        private EnemyParameters _enemyParameters;

        private IAnimationController animationController;
        private Attack attack;
        private int healthPoints;
        private Rigidbody2D rigidbody;
        private Targeting targeting;

        private void Awake()
        {
            animationController = GetComponentInChildren<IAnimationController>();
            rigidbody = GetComponent<Rigidbody2D>();
            targeting = GetComponent<Targeting>();
            attack = GetComponent<Attack>();
            healthPoints = EnemyParameters.MaxHealthPoints;

            stateMachine = new StateMachine();

            MoveState moveState = new MoveState(gameObject);
            IddleState iddleState = new IddleState(gameObject);
            ChaseState chaseState = new ChaseState(gameObject);
            AttackState attackState = new AttackState(gameObject);
            AlertState alertState = new AlertState(gameObject);

            stateMachine.SetState(iddleState);

            stateMachine.AddTransition(iddleState, moveState, iddleState.TimeEnded);
            stateMachine.AddTransition(iddleState, alertState, iddleState.TimeEnded);

            stateMachine.AddTransition(moveState, chaseState, () => targeting.hasTarget);
            stateMachine.AddTransition(moveState, alertState, () => targeting.hasTarget);

            stateMachine.AddTransition(chaseState, attackState, () => attack.isInRange);
            stateMachine.AddTransition(chaseState, moveState, () => !targeting.hasTarget);

            stateMachine.AddTransition(attackState, moveState, () => !targeting.hasTarget);
            stateMachine.AddTransition(attackState, chaseState, () => !attack.isInRange && targeting.hasTarget);

            stateMachine.AddTransition(alertState, moveState, () => !targeting.hasTarget);
            stateMachine.AddTransition(alertState, chaseState,
                () => alertState.TimeEnded() && targeting.hasTarget && !attack.isInRange);
            stateMachine.AddTransition(alertState, attackState,
                () => alertState.TimeEnded() && targeting.hasTarget && attack.isInRange);
        }

        private void Update()
        {
            stateMachine.Tick();
        }

        public StateMachine stateMachine { get; set; }
        public bool movingRight { get; set; } = true;
        public bool alive { get; set; } = true;
        public EnemyParameters EnemyParameters => _enemyParameters;

        public void Interfacinha()
        {
        }

        // TODO: add knockback if necessary
        public void Hit(int damage, Transform attacker)
        {
            if (healthPoints <= 0)
            {
                gameObject.SetActive(false);
                alive = false;
            }
            else
            {
                animationController.TriggerAnimationHit();
            }
        }
    }
}