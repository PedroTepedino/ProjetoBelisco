using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    public class GolemSimple : MonoBehaviour, IHittable, IEnemyStateMachine
    {
        [SerializeField] [AssetsOnly] [InlineEditor]
        private EnemyParameters _enemyParameters;

        private IAnimationController animationController;
        private Attack attack;
        private int healthPoints;
        private Rigidbody2D ownerRigidbody;
        private Targeting targeting;

        private void Awake()
        {
            animationController = GetComponentInChildren<IAnimationController>();
            ownerRigidbody = GetComponent<Rigidbody2D>();
            targeting = GetComponent<Targeting>();
            attack = GetComponent<Attack>();
            healthPoints = EnemyParameters.MaxHealthPoints;

            stateMachine = new StateMachine();

            MoveState moveState = new MoveState(gameObject);
            IddleState iddleState = new IddleState(gameObject);
            ChaseState chaseState = new ChaseState(gameObject);
            AlertState alertState = new AlertState(gameObject);

            stateMachine.SetState(iddleState);

            stateMachine.AddTransition(iddleState, moveState, () => iddleState.TimeEnded() && !targeting.hasTarget);
            stateMachine.AddTransition(iddleState, alertState, () => iddleState.TimeEnded() && targeting.hasTarget);

            stateMachine.AddTransition(moveState, iddleState, moveState.TimeEnded);
            stateMachine.AddTransition(moveState, alertState, () => targeting.hasTarget);

            stateMachine.AddTransition(chaseState, iddleState, () => !targeting.hasTarget);

            stateMachine.AddTransition(alertState, iddleState, () => !targeting.hasTarget);
            stateMachine.AddTransition(alertState, chaseState, () => alertState.TimeEnded() && targeting.hasTarget);
        }

        private void Update()
        {
            stateMachine.Tick();
        }

        private void OnCollisionEnter2D(Collision2D hit)
        {
            Player player = hit.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Hit(1);
                ownerRigidbody.velocity = Vector2.zero;
            }
        }

        public StateMachine stateMachine { get; set; }
        public bool movingRight { get; set; } = true;
        public bool alive { get; set; } = true;
        public EnemyParameters EnemyParameters => _enemyParameters;

        public void Interfacinha()
        {
        }

        public void Hit(int damage)
        {
            healthPoints -= damage;
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