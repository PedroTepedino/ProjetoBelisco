using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class GrounderEnemy : MonoBehaviour
    {
        private Vector3 grounderCenter;
        private float grounderDistance;
        private LayerMask groundLayerMask;
   
        private IEnemyStateMachine controller;
        private Vector3 checkerCenter;
        
        public bool isGrounded { get; private set; } = false;


        private void Awake() {
            controller = GetComponent<IEnemyStateMachine>();
            grounderCenter = controller.EnemyParameters.GrounderCenter;
            grounderDistance = controller.EnemyParameters.GrounderDistance;
            groundLayerMask = controller.EnemyParameters.GrounderLayerMask;
        }

        private void Update()
        {
            isGrounded = GroundCheck();
        }
    
        private bool GroundCheck()
        {
            RaycastHit2D raycastHit2D;
            checkerCenter = this.transform.position + new Vector3(controller.movingRight ? grounderCenter.x : -grounderCenter.x , grounderCenter.y, 0);
            raycastHit2D = Physics2D.Raycast(checkerCenter, Vector2.down, grounderDistance, groundLayerMask);

            return raycastHit2D.collider != null;
        }

        protected void OnDrawGizmos()
        {
            // Guard sentence
            if (controller == null)
                return;
        
            Gizmos.color = isGrounded ? Color.green : Color.red;
            if(controller.movingRight){
                Gizmos.DrawLine(this.transform.position + grounderCenter, this.transform.position + new Vector3(grounderCenter.x, grounderCenter.y - grounderDistance, 0));
            }else{
                Gizmos.DrawLine(this.transform.position + new Vector3(-grounderCenter.x, grounderCenter.y , 0), this.transform.position + new Vector3(-grounderCenter.x, grounderCenter.y - grounderDistance, 0));
            }
        }
    }
}
