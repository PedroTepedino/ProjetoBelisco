using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Targeting : MonoBehaviour
    {
        [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _targetingCenter;
        [FoldoutGroup("Parameters")] [SerializeField] private Vector3 bossZoneCornerA;
        [FoldoutGroup("Parameters")] [SerializeField] private Vector3 bossZoneCornerB;
        [FoldoutGroup("Parameters")] [SerializeField] [EnumToggleButtons] private LayerMask _targetingLayerMask;
        

        private IEnemyStateMachine controller;
        private Attack attack;
        private Vector3 _checkerCenter;
        private bool bossEnemy = false;
        private float lookingRange;

        public Transform target { get; private set; } = null;
        public bool hasTarget { get; private set; } = false;
        

        void Start()
        {
            controller = GetComponent<IEnemyStateMachine>();
            attack = GetComponent<Attack>();
            bossEnemy = controller.EnemyParameters.IsBoss;
            lookingRange = controller.EnemyParameters.LookingRange;
            // if (bossEnemy){
            //     hasTarget = true;
            //     target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            // }
        }

        void Update()
        {
            
            // if (!bossEnemy)
            // {
                hasTarget = TargetingAction();
            // }
            // else if(target == null)
            // {
            //     hasTarget = true;
            //     target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            // }
            if (hasTarget)
            {
                attack.isInRange = ((Vector2.Distance(this.transform.position, target.position) > attack.attackRange) ? false : true);
            }
        }

        private bool TargetingAction(){
            if (bossEnemy)
            {
                var hitObjects = Physics2D.OverlapAreaAll(bossZoneCornerA, bossZoneCornerB, _targetingLayerMask);
                target = ((CheckHit(hitObjects) != null) ? CheckHit(hitObjects).transform : null);
                return (target != null)/* ? true : false*/;
            }
            else
            {
                var hitObject = Physics2D.Raycast(this.transform.position, controller.movingRight ? Vector2.right : Vector2.left, lookingRange, _targetingLayerMask);
                if (hitObject.collider != null)
                {
                    if (hitObject.transform.gameObject.GetComponent<IHittable>() != null)
                    {
                        target = hitObject.transform;
                        return true;
                    }
                    else
                    {
                        target = null;
                        return false;
                    }
                }
                else
                {
                    target = null;
                    return false;
                }
            }
        }

        private Collider2D CheckHit(Collider2D[] hits)
        {
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    return hit;
                }
            }
            return null;
        }

        protected void OnDrawGizmos() {
            // Guard sentence
            if (controller == null)
                return;
        
            Gizmos.color = Color.yellow;
            
            if (controller.movingRight)
            {
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(lookingRange,0 , 0));
                Gizmos.DrawLine(this.transform.position+ new Vector3(0, 0.5f, 0), this.transform.position + new Vector3(attack.attackRange, 0.5f, 0));            
            }
            else
            {
                Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-lookingRange,0 , 0));
                Gizmos.DrawLine(this.transform.position+ new Vector3(0, 0.5f, 0), this.transform.position + new Vector3(-attack.attackRange, 0.5f, 0));
            }
        }
    }
}
