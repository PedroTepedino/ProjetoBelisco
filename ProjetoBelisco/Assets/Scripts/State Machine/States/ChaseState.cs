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
    private EnemyAttack enemyAttack;

    public ChaseState(GameObject owner){
        ownerGameObject = owner;
        controllerOwner = owner.GetComponent<EnemyController>();
    }
    public void EnterState(){
        controllerOwner.actualState = "chase";
        ownerRigidbody = ownerGameObject.GetComponent<Rigidbody2D>();
        enemyAttack = ownerGameObject.GetComponent<EnemyAttack>();
        grounder = ownerGameObject.GetComponent<EnemyGrounder>();
        wallCheck = ownerGameObject.GetComponent<EnemyWallChecker>();
        target = controllerOwner.targeting.target;
    }

    public void RunState()
    {
        target = controllerOwner.targeting.target;

        if (target != null)
        {
            Debug.Log(target);
            Debug.Log(Vector2.Distance(ownerGameObject.transform.position, target.position) <= enemyAttack.attackRange);
            if (Vector2.Distance(ownerGameObject.transform.position, target.position) <= enemyAttack.attackRange)
            {
                
                if (controllerOwner.actualState != "attack")
                {
                    Debug.Log("atk");
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
                        movement.Set(controllerOwner.movingSpeed, controllerOwner.rigidbody.velocity.y);
                        controllerOwner.rigidbody.velocity = movement;
                    }
                }
                else if (direction.x > 0)
                {
                    if (grounder.isGrounded && wallCheck.wallAhead)
                    {
                        controllerOwner.movingRight = false;
                        movement.Set(-controllerOwner.movingSpeed, controllerOwner.rigidbody.velocity.y);
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
