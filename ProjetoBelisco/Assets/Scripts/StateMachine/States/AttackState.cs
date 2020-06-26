﻿using GameScripts.Enemies;
using UnityEngine;

namespace GameScripts.StateMachine.States
{
    public class AttackState : IState
    {
        private GameObject ownerGameObject;
        private Transform target;
        private Controller controllerOwner;
        private Rigidbody2D ownerRigidbody;
        private Grounder grounder;
        private WallChecker wallCheck;
        private Attack _attack;
        private float timer;

        public AttackState(GameObject owner)
        {
            ownerGameObject = owner;
            controllerOwner = owner.GetComponent<Controller>();       
        }

        public void EnterState()
        {
            controllerOwner.actualState = "attack";
            target = controllerOwner.targeting.target;
            ownerRigidbody = ownerGameObject.GetComponent<Rigidbody2D>();
            _attack = ownerGameObject.GetComponent<Attack>();
            grounder = ownerGameObject.GetComponent<Grounder>();
            wallCheck = ownerGameObject.GetComponent<WallChecker>();
            timer = 0;
            _attack.IsInRange = true;
        }

        public void ExitState()
        {
            _attack.IsInRange = false;
        }

        public void RunState()
        {
            target = controllerOwner.targeting.target;
            timer += Time.deltaTime;
            
            if(target != null)
            {
                if (timer >= _attack.attackSpeed)
                {
                    _attack.AttackAction(target);
                    timer = 0;                       
                }
            }
        }
    }
}
