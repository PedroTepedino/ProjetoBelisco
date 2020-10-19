using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Targeting : MonoBehaviour
    {

        private Vector3 targetingCenter;
        private Vector3 bossZoneCornerA;
        private Vector3 bossZoneCornerB;
        private LayerMask targetingLayerMask;
        

        private IEnemyStateMachine controller;
        private Attack attack;
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
            targetingCenter = controller.EnemyParameters.TargetingCenter;
            bossZoneCornerA = controller.EnemyParameters.BossZoneCornerA;
            bossZoneCornerB = controller.EnemyParameters.BossZoneCornerB;
            targetingLayerMask = controller.EnemyParameters.TargetingLayerMask;
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
                attack.isInRange = !(Vector2.Distance(this.transform.position, target.position) > attack.attackRange);
            }
        }

        private bool TargetingAction(){
            if (bossEnemy)
            {
                var hitObjects = Physics2D.OverlapAreaAll(bossZoneCornerA, bossZoneCornerB, targetingLayerMask);
                target = CheckHit(hitObjects);
                return (target != null);
            }
            else
            {
                var hitObject = Physics2D.Raycast(this.transform.position + targetingCenter, controller.movingRight ? Vector2.right : Vector2.left, lookingRange, targetingLayerMask);
                target = CheckHit(hitObject);
                return (target != null);
            }
        }

        private Transform CheckHit(Collider2D[] hits)
        {
            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject.layer == LayerMask.GetMask("Player"))
                {
                    return hit.transform;
                }
            }
            return null;
        }

        private Transform CheckHit(RaycastHit2D hit)
        {
            if(hit.transform != null)
            {
                if (hit.transform.gameObject.GetComponent<Player>() != null)
                {
                    return hit.transform;
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
