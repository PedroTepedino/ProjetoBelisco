 using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class WallChecker : MonoBehaviour
    {
        private EnemyStateMachine controller;

        
        private Vector3 wallCheckerTop = Vector3.zero;    
        private Vector3 wallCheckerCenter = Vector3.zero;
        private Vector3 wallCheckerBottom = Vector3.zero;
        private float checkerDistance;
        private LayerMask wallLayerMask;
        
        private Vector3 checkerTop;
        private Vector3 checkerCenter;
        private Vector3 checkerBottom;

        public bool wallAhead { get; private set; } = false;
        public bool wallTop = false;
        public bool wallCenter = false;
        public bool wallBottom = false;
    

        private void Awake() {
            controller = GetComponent<EnemyStateMachine>();
            wallCheckerTop = controller.EnemyParameters.WallCheckerTop;
            wallCheckerCenter = controller.EnemyParameters.WallCheckerCenter;
            wallCheckerTop = controller.EnemyParameters.WallCheckerBottom;
            checkerDistance = controller.EnemyParameters.WallCheckerDistance;
            wallLayerMask = controller.EnemyParameters.WallCheckerLayerMask;


        }
        private void Update()
        {
            wallAhead = WallCheck();
        }
    
        private bool WallCheck()
        {
            RaycastHit2D raycastHit2DTop;
            checkerTop = this.transform.position + new Vector3(controller.movingRight ? wallCheckerTop.x : -wallCheckerTop.x, wallCheckerTop.y, 0);
            raycastHit2DTop = Physics2D.Raycast(checkerTop, controller.movingRight ? Vector2.right : Vector2.left, checkerDistance, wallLayerMask);
            wallTop = raycastHit2DTop.collider != null ? true : false;

            RaycastHit2D raycastHit2DCenter;
            checkerCenter = this.transform.position + new Vector3(controller.movingRight ? wallCheckerCenter.x : -wallCheckerCenter.x, wallCheckerCenter.y, 0);
            raycastHit2DCenter = Physics2D.Raycast(checkerCenter, controller.movingRight ? Vector2.right : Vector2.left, checkerDistance, wallLayerMask);
            wallCenter = raycastHit2DCenter.collider != null ? true : false;

            RaycastHit2D raycastHit2DBottom;
            checkerBottom = this.transform.position + new Vector3(controller.movingRight ? wallCheckerBottom.x : -wallCheckerBottom.x, wallCheckerBottom.y, 0);
            raycastHit2DBottom = Physics2D.Raycast(checkerBottom, controller.movingRight ? Vector2.right : Vector2.left, checkerDistance, wallLayerMask);
            wallBottom = raycastHit2DBottom.collider != null ? true : false;
        
            if(!wallTop && !wallCenter && !wallBottom){
                wallAhead = false;
            }
            else
            {
                wallAhead = true;
            }
            return wallAhead;

        }

        protected void OnDrawGizmos()
        {
            // Guard sentence
            if (controller == null)
                return;
        
            Gizmos.color = wallAhead ? Color.red : Color.green;

            if(controller.movingRight){
                Gizmos.DrawLine(this.transform.position + wallCheckerTop, this.transform.position + new Vector3(wallCheckerTop.x + checkerDistance, wallCheckerTop.y, 0));
                Gizmos.DrawLine(this.transform.position + wallCheckerCenter, this.transform.position + new Vector3(wallCheckerCenter.x + checkerDistance, wallCheckerCenter.y, 0));
                Gizmos.DrawLine(this.transform.position + wallCheckerBottom, this.transform.position + new Vector3(wallCheckerBottom.x + checkerDistance, wallCheckerBottom.y, 0));

            }
            else
            {
                Gizmos.DrawLine(this.transform.position + new Vector3(-wallCheckerTop.x, wallCheckerTop.y, 0), this.transform.position + new Vector3(-wallCheckerTop.x - checkerDistance, wallCheckerTop.y, 0));
                Gizmos.DrawLine(this.transform.position + new Vector3(-wallCheckerCenter.x, wallCheckerCenter.y, 0), this.transform.position + new Vector3(-wallCheckerCenter.x - checkerDistance, wallCheckerCenter.y, 0));
                Gizmos.DrawLine(this.transform.position + new Vector3(-wallCheckerBottom.x, wallCheckerBottom.y, 0), this.transform.position + new Vector3(-wallCheckerBottom.x - checkerDistance, wallCheckerBottom.y, 0));  
            }
        }
    }
}
