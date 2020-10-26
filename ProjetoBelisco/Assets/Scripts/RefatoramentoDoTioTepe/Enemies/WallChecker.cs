﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class WallChecker : MonoBehaviour
    {
        private IEnemyStateMachine controller;

        private Vector2 wallCheckerCenter = Vector2.zero;

        private float checkerDistance;
        private LayerMask wallLayerMask;

        private Vector2 checkerCenter;

        public bool wallAhead { get; private set; } = false;

        private void Awake()
        {
            controller = GetComponent<IEnemyStateMachine>();
            checkerDistance = controller.EnemyParameters.WallCheckerDistance;
            wallLayerMask = controller.EnemyParameters.WallCheckerLayerMask;
            wallCheckerCenter = controller.EnemyParameters.WallCheckerCenter;
        }
        private void Update()
        {
            wallAhead = WallCheck();
        }

        private bool WallCheck()
        {

            RaycastHit2D raycastHit2D;
            checkerCenter = this.transform.position + new Vector3(controller.movingRight ? wallCheckerCenter.x : -wallCheckerCenter.x, wallCheckerCenter.y, 0);
            raycastHit2D = Physics2D.Raycast(checkerCenter, controller.movingRight ? Vector2.right : Vector2.left, checkerDistance, wallLayerMask);
            wallAhead = raycastHit2D.collider != null ? true : false;
            return wallAhead;

        }

        protected void OnDrawGizmos()
        {
            // Guard sentence
            if (controller == null)
                return;

            Gizmos.color = wallAhead ? Color.red : Color.green;

            if (controller.movingRight)
            {
                Gizmos.DrawLine(this.transform.position + new Vector3(wallCheckerCenter.x, wallCheckerCenter.y, 0), this.transform.position + new Vector3(wallCheckerCenter.x + checkerDistance, wallCheckerCenter.y, 0));

            }
            else
            {
                Gizmos.DrawLine(this.transform.position + new Vector3(-wallCheckerCenter.x, wallCheckerCenter.y, 0), this.transform.position + new Vector3(-wallCheckerCenter.x - checkerDistance, wallCheckerCenter.y, 0));

            }
        }
    }
}