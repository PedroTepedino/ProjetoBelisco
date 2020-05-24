using GameScripts.StateMachine.States;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameScripts.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Life))]
    [RequireComponent(typeof(WallChecker))]
    [RequireComponent(typeof(Grounder))]
    [RequireComponent(typeof(Targeting))]
    [RequireComponent(typeof(Attack))]
/*Class: EnemyController
 * Abstract Class that describes the essential components of every enemy.
 */
    public abstract class Controller : MonoBehaviour
    {
        [EnumPaging]public LayerMask layerTargeting;
        public StateMachine.StateMachine stateMachine;
        public Rigidbody2D rigidbody;
        public Targeting targeting;
        public string actualState;
        public bool movingRight = true;
        public float movingSpeed = 5;
        public float lookingRange = 5;
        public float alertAnimationTime = 0;
        public float maxStopTime = 0;
        public float minTimeBetweenStops = 3;


        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            stateMachine = gameObject.AddComponent<StateMachine.StateMachine>();
            this.stateMachine.ChangeState(new MoveState(this.gameObject));
            targeting = GetComponent<Targeting>();
        }
    
        public abstract void Update();
    }
}
