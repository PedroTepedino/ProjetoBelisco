using GameScripts.Enemies;
using UnityEngine;

namespace GameScripts.StateMachine.States
{
    public class AlertState : IState
    {

        private GameObject ownerGameObject;
        private Transform target;
        private Controller controllerOwner;
        private float timer;
        private float alertAnimationTime;

        public AlertState(GameObject owner)
        {
            ownerGameObject = owner;
            controllerOwner = owner.GetComponent<Controller>();       
        }

        public void EnterState()
        {
            controllerOwner.actualState = "alert";
            target = controllerOwner.targeting.target;
            alertAnimationTime = controllerOwner.alertAnimationTime;
        }

        public void ExitState()
        {
       
        }

        public void RunState()
        {
            target = controllerOwner.targeting.target;
            timer += Time.deltaTime;
        
            if(target != null)
            {
                if (timer >= alertAnimationTime)
                {
                    if (controllerOwner.actualState != "chase")
                    {
                        controllerOwner.stateMachine.ChangeState(new ChaseState(ownerGameObject));
                    }
                }
            }
            else
            {
                if (controllerOwner.actualState != "move")
                {
                    controllerOwner.stateMachine.ChangeState(new MoveState(ownerGameObject));
                }
            }
        }
    }
}
