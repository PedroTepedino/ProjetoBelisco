using UnityEngine;

namespace Belisco
{
    public class WallChecker : MonoBehaviour
    {
        private Vector2 checkerCenter;

        private float checkerDistance;
        private EnemyStateMachine controller;

        private Vector2 wallCheckerCenter = Vector2.zero;
        private LayerMask wallLayerMask;

        public bool wallAhead { get; private set; }

        private void Awake()
        {
            controller = GetComponent<EnemyStateMachine>();
        }

        private void Start()
        {
            checkerDistance = controller.EnemyParameters.WallCheckerDistance;
            wallLayerMask = controller.EnemyParameters.WallCheckerLayerMask;
            wallCheckerCenter = controller.EnemyParameters.WallCheckerCenter;
        }

        private void Update()
        {
            wallAhead = WallCheck();
        }

        protected void OnDrawGizmos()
        {
            // Guard sentence
            if (controller == null)
                return;

            Gizmos.color = wallAhead ? Color.red : Color.green;

            if (controller.movingRight)
                Gizmos.DrawLine(transform.position + new Vector3(wallCheckerCenter.x, wallCheckerCenter.y, 0),
                    transform.position + new Vector3(wallCheckerCenter.x + checkerDistance, wallCheckerCenter.y, 0));
            else
                Gizmos.DrawLine(transform.position + new Vector3(-wallCheckerCenter.x, wallCheckerCenter.y, 0),
                    transform.position + new Vector3(-wallCheckerCenter.x - checkerDistance, wallCheckerCenter.y, 0));
        }

        private bool WallCheck()
        {
            RaycastHit2D raycastHit2D;
            checkerCenter = transform.position +
                new Vector3(controller.movingRight ? wallCheckerCenter.x : -wallCheckerCenter.x,
                    wallCheckerCenter.y, 0);
            raycastHit2D = Physics2D.Raycast(checkerCenter, controller.movingRight ? Vector2.right : Vector2.left,
                checkerDistance, wallLayerMask);
            wallAhead = raycastHit2D.collider != null ? true : false;
            return wallAhead;
        }
    }
}