﻿using GameScripts.Enemies;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class AttackState : IState
    {
        private GameObject ownerGameObject;
        private Transform target;
        private Attack attack;
        private Targeting targeting;
        private AnimationController animationController;
        private float timer;

        public AttackState(GameObject owner)
        {
            ownerGameObject = owner; 
            attack = ownerGameObject.GetComponent<Attack>();
            targeting = ownerGameObject.GetComponent<Targeting>(); 
            animationController = ownerGameObject.GetComponent<AnimationController>(); 
        }

        public void OnEnter()
        {
            target = targeting.target;
            timer = 0f;
        }

        public void OnExit()
        {
            
        }

        public void Tick()
        {
            target = targeting.target;
            timer += Time.deltaTime;
            
            if(target != null)
            {
                if (timer >= attack.attackSpeed)
                {
                    attack.AttackAction(target);
                    animationController.TriggerAnimationAttack();
                    timer = 0f;                       
                }
            }
        }
    }
}
