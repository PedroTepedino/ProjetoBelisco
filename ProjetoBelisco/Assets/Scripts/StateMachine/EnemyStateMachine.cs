using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    public abstract class EnemyStateMachine : MonoBehaviour, IHittable
    {
        public EnemyParameters EnemyParameters => _enemyParameters;
        public StateMachine stateMachine;
        public bool movingRight = true;
        public bool alive = true;

        [SerializeField][AssetsOnly][InlineEditor] protected EnemyParameters _enemyParameters;
        protected EnemyAnimationController animationController;
        protected Attack attack;
        protected int healthPoints;
        protected Rigidbody2D enemyRigidbody;
        protected Targeting targeting;

        protected virtual void Awake()
        {
            animationController = GetComponentInChildren<EnemyAnimationController>();
            enemyRigidbody = GetComponent<Rigidbody2D>();
            targeting = GetComponent<Targeting>();
            attack = GetComponent<Attack>();
            healthPoints = EnemyParameters.MaxHealthPoints;

            stateMachine = new StateMachine();
        }

        protected virtual void Update()
        {
            stateMachine.Tick();
        }

        public virtual void Hit(int damage, Transform attacker)
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

        protected virtual void OnCollisionEnter2D(Collision2D hit)
        {
            Player player = hit.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Hit(1, this.transform);
                enemyRigidbody.velocity = Vector2.zero;
            }
        }
    }
}