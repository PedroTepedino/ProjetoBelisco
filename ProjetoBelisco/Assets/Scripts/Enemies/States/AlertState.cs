﻿using UnityEngine;

namespace Belisco
{
    public class AlertState : IState
    {
        private float alertAnimationTime;
        private readonly IEnemyStateMachine ownerController;

        private GameObject ownerGameObject;
        private Transform target;
        private float timer;

        public AlertState(GameObject owner)
        {
            ownerGameObject = owner;
            ownerController = owner.GetComponent<IEnemyStateMachine>();
        }

        public void OnEnter()
        {
            timer = 0f;
            alertAnimationTime = ownerController.EnemyParameters.AlertAnimationTime;
        }

        public void OnExit()
        {
        }

        public void Tick()
        {
            timer += Time.deltaTime;
        }

        public bool TimeEnded()
        {
            return timer > alertAnimationTime;
        }
    }
}