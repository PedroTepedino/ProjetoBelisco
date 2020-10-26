using UnityEngine;

namespace Belisco
{
    public class GrounderEnemy : MonoBehaviour
    {
        private Vector2 checkerCenter;

        private IEnemyStateMachine controller;
        private Vector2 grounderCenter;
        private float grounderDistance;
        private LayerMask groundLayerMask;

        public bool isGrounded { get; private set; }

        private void Awake()
        {
            controller = GetComponent<IEnemyStateMachine>();
            grounderCenter = controller.EnemyParameters.GrounderCenter;
            grounderDistance = controller.EnemyParameters.GrounderDistance;
            groundLayerMask = controller.EnemyParameters.GrounderLayerMask;
        }

        private void Update()
        {
            isGrounded = GroundCheck();
        }

        protected void OnDrawGizmos()
        {
            // Guard sentence
            if (controller == null)
                return;

            Gizmos.color = isGrounded ? Color.green : Color.red;
            if (controller.movingRight)
                Gizmos.DrawLine(transform.position + new Vector3(grounderCenter.x, grounderCenter.y, 0),
                    transform.position + new Vector3(grounderCenter.x, grounderCenter.y - grounderDistance, 0));
            else
                Gizmos.DrawLine(transform.position + new Vector3(-grounderCenter.x, grounderCenter.y, 0),
                    transform.position + new Vector3(-grounderCenter.x, grounderCenter.y - grounderDistance, 0));
        }

        private bool GroundCheck()
        {
            RaycastHit2D raycastHit2D;
            checkerCenter = transform.position +
                            new Vector3(controller.movingRight ? grounderCenter.x : -grounderCenter.x, grounderCenter.y,
                                0);
            raycastHit2D = Physics2D.Raycast(checkerCenter, Vector2.down, grounderDistance, groundLayerMask);

            return raycastHit2D.collider != null;
        }
    }
}