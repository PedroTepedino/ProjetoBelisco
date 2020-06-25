using GameScripts.Enemies;
using UnityEngine;

namespace GameScripts.StateMachine.States
{
    public class ChaseState : IState
    {
        private GameObject ownerGameObject;
        private Transform target;
        private Controller controllerOwner;
        private Rigidbody2D ownerRigidbody;
        private Grounder grounder;
        private WallChecker wallCheck;
        private Vector2 movement;
        private Attack _attack;

        public ChaseState(GameObject owner){
            ownerGameObject = owner;
            controllerOwner = owner.GetComponent<Controller>();
        }
        public void EnterState(){
            controllerOwner.actualState = "chase";
            ownerRigidbody = ownerGameObject.GetComponent<Rigidbody2D>();
            _attack = ownerGameObject.GetComponent<Attack>();
            grounder = ownerGameObject.GetComponent<Grounder>();
            wallCheck = ownerGameObject.GetComponent<WallChecker>();
            target = controllerOwner.targeting.target;
        }

        public void RunState()
        {
            target = controllerOwner.targeting.target;

            if (target != null)
            {
                Vector2 direction = (target.position - ownerGameObject.transform.position).normalized;
                if (direction.x < 0)//direita
                {
                    if(controllerOwner.movingRight){
                        Move();
                    }
                    else
                    {
                        Flip();
                    }                       
                }
                else if (direction.x > 0)
                {
                    if(!controllerOwner.movingRight){
                        Move();
                    }
                    else
                    {
                        Flip();
                    }
                }

                // if (Vector2.Distance(ownerGameObject.transform.position, target.position) <= _attack.attackRange)
                // {
                //     if (controllerOwner.actualState != "attack")
                //     {
                //         controllerOwner.stateMachine.ChangeState(new AttackState(ownerGameObject));
                //     }
                // }
                // else
                // {
                //     Vector2 direction = (target.position - ownerGameObject.transform.position).normalized;
                //     if (direction.x < 0)//direita
                //     {
                //         if(controllerOwner.movingRight){
                //             Move();
                //         }
                //         else
                //         {
                //             Flip();
                //         }
                //         // if (grounder.isGrounded && wallCheck.wallAhead)
                //         // {
                //         //     Debug.Log("anda direita");
                //         //     controllerOwner.movingRight = true;
                //         //     movement.Set(controllerOwner.movingSpeed, ownerRigidbody.velocity.y);
                //         //     ownerRigidbody.velocity = movement;
                //         // }

                //     }
                //     else if (direction.x > 0)
                //     {
                //         if(!controllerOwner.movingRight){
                //             Move();
                //         }
                //         else
                //         {
                //             Flip();
                //         }
                //         // if (grounder.isGrounded && wallCheck.wallAhead)
                //         // {
                //         //     Debug.Log("anda esquerda");
                //         //     controllerOwner.movingRight = false;
                //         //     movement.Set(-controllerOwner.movingSpeed, ownerRigidbody.velocity.y);
                //         //     ownerRigidbody.velocity = movement;
                //         // }
                //     }
                // }

                //  if (Vector2.Distance(ownerGameObject.transform.position, target.position) > controllerOwner.lookingRange)
                // {
                //     if (controllerOwner.actualState != "move")
                //     {
                //         controllerOwner.stateMachine.ChangeState(new MoveState(ownerGameObject));
                //     }
                // }
            }
            // else
            // {
            //     if (controllerOwner.actualState != "move")
            //     {
            //         controllerOwner.stateMachine.ChangeState(new MoveState(ownerGameObject));
            //     }
            // }
        }

        public void ExitState()
        {
        
        }

        private void Move()
        {
            movement.Set(controllerOwner.movingRight ? controllerOwner.movingSpeed : -controllerOwner.movingSpeed, ownerRigidbody.velocity.y);
            ownerRigidbody.velocity = movement;
        }

        private void Flip()
        {
            ownerGameObject.transform.eulerAngles = new Vector3(0, controllerOwner.movingRight ? -180 : 0, 0);
            controllerOwner.movingRight = controllerOwner.movingRight ? false : true;
        }
    }
}
