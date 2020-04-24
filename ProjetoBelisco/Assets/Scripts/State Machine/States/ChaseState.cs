using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private GameObject ownerGameObject;
    private Transform target;
    private EnemyController controllerOwner;
    private Rigidbody2D ownerRigidbody;
    private EnemyGrounder grounder;
    private EnemyWallChecker wallCheck;
    private Vector2 movement;

    public ChaseState(GameObject owner){
        ownerGameObject = owner;
        controllerOwner = owner.GetComponent<EnemyController>();
    }
    public void EnterState(){
        controllerOwner.actualState = "chase";
        ownerRigidbody = ownerGameObject.GetComponent<Rigidbody2D>();
        grounder = ownerGameObject.GetComponent<EnemyGrounder>();
        wallCheck = ownerGameObject.GetComponent<EnemyWallChecker>();
        target = controllerOwner.targeting.target;
    }

    public void RunState()
    {
        target = controllerOwner.targeting.target;

        if (target != null)
        {
            if (Vector2.Distance(ownerGameObject.transform.position, target.position) <= controllerOwner.attackRange)
            {
                if (controllerOwner.actualState != "attack")
                {
                    controllerOwner.stateMachine.ChangeState(new AttackState(ownerGameObject));
                }
            }
            else
            {
                Vector2 direction = (target.position - ownerGameObject.transform.position).normalized;
                if (direction.x < 0)//direita
                {
                    if (grounder.isGrounded && wallCheck.wallAhead)
                    {
                        controllerOwner.movingRight = true;
                        movement.Set(controllerOwner.movingSpeed * Time.deltaTime, controllerOwner.rigidbody.velocity.y);
                        controllerOwner.rigidbody.velocity = movement;
                    }
                }
                else if (direction.x > 0)
                {
                    if (grounder.isGrounded && wallCheck.wallAhead)
                    {
                        controllerOwner.movingRight = true;
                        movement.Set(-controllerOwner.movingSpeed * Time.deltaTime, controllerOwner.rigidbody.velocity.y);
                        controllerOwner.rigidbody.velocity = movement;
                    }
                }
            }
            
        }
        else
        {
            controllerOwner.stateMachine.ChangeState(new MoveState(ownerGameObject));
        }
    }

    public void ExitState()
    {
        
    }
}
