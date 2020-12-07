﻿using UnityEngine;

namespace Belisco
{
    public class Targeting : MonoBehaviour
    {
        private Attack attack;
        private bool bossEnemy;
        private Vector2 bossZoneCornerA;
        private Vector2 bossZoneCornerB;

        private EnemyStateMachine controller;
        private float lookingRange;

        private Vector2 targetingCenter;
        private LayerMask targetingLayerMask;

        public Transform target { get; private set; }
        public bool hasTarget { get; private set; }

        private void Awake()
        {
            controller = GetComponent<EnemyStateMachine>();
            attack = GetComponent<Attack>();
        }

        private void Start()
        {
            bossEnemy = controller.EnemyParameters.IsBoss;
            lookingRange = controller.EnemyParameters.LookingRange;
            targetingCenter = controller.EnemyParameters.TargetingCenter;
            bossZoneCornerA = controller.EnemyParameters.BossZoneCornerA;
            bossZoneCornerB = controller.EnemyParameters.BossZoneCornerB;
            targetingLayerMask = controller.EnemyParameters.TargetingLayerMask;
        }

        private void Update()
        {
            hasTarget = TargetingAction();

            if (hasTarget)
            {
                if (bossEnemy)
                {
                    attack.isInRange = true;
                }
                else
                {
                    attack.isInRange = !(Vector2.Distance(transform.position, target.position) > attack.attackRange);
                }
            }
        }

        protected void OnDrawGizmos()
        {
            // Guard sentence
            if (controller == null)
                return;

            Gizmos.color = Color.yellow;

            if (controller.movingRight)
            {
                Gizmos.DrawLine(transform.position, transform.position + new Vector3(lookingRange, 0, 0));
                Gizmos.DrawLine(transform.position + new Vector3(0, 0.5f, 0),
                    transform.position + new Vector3(attack.attackRange, 0.5f, 0));
            }
            else
            {
                Gizmos.DrawLine(transform.position, transform.position + new Vector3(-lookingRange, 0, 0));
                Gizmos.DrawLine(transform.position + new Vector3(0, 0.5f, 0),
                    transform.position + new Vector3(-attack.attackRange, 0.5f, 0));
            }
        }

        private bool TargetingAction()
        {
            if (bossEnemy)
            {
                var hitObjects = Physics2D.OverlapAreaAll(bossZoneCornerA, bossZoneCornerB, targetingLayerMask);
                target = CheckHit(hitObjects);
                return target != null;
            }

            RaycastHit2D hitObject =
                Physics2D.Raycast(transform.position + new Vector3(targetingCenter.x, targetingCenter.y, 0),
                    controller.movingRight ? Vector2.right : Vector2.left, lookingRange, targetingLayerMask);
            target = CheckHit(hitObject);
            return target != null;
        }

        private Transform CheckHit(Collider2D[ ] hits)
        {
            foreach (Collider2D hit in hits)
                if (hit.gameObject.layer == LayerMask.GetMask("Player"))
                    return hit.transform;
            return null;
        }

        private Transform CheckHit(RaycastHit2D hit)
        {
            if (hit.transform != null)
                if (hit.transform.gameObject.GetComponent<Player>() != null)
                    return hit.transform;
            return null;
        }
    }
}