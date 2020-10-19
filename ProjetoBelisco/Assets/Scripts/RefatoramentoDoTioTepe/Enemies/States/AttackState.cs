using GameScripts.Enemies;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class AttackState : IState
    {
        private GameObject ownerGameObject;
        private Transform target;
        private Attack attack;
        private Targeting targeting;
        private IAnimationController animationController;
        private float timer;

        public AttackState(GameObject owner)
        {
            ownerGameObject = owner; 
            attack = ownerGameObject.GetComponent<Attack>();
            targeting = ownerGameObject.GetComponent<Targeting>(); 
            animationController = ownerGameObject.GetComponent<IAnimationController>(); 
        }

        public void OnEnter()
        {
            target = targeting.target;
            timer = attack.attackSpeed;
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
