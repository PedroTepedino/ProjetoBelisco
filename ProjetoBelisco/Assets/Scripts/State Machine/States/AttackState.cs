using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private GameObject ownerGameObject;
    private Transform target;
    private EnemyController controllerOwner;
    private Rigidbody2D ownerRigidbody;
    private EnemyGrounder grounder;
    private EnemyWallChecker wallCheck;
    private EnemyAttack enemyAttack;
    private float timer;

    public AttackState(GameObject owner)
    {
        ownerGameObject = owner;
        controllerOwner = owner.GetComponent<EnemyController>();       
    }

    public void EnterState()
    {
        controllerOwner.actualState = "attack";
        target = controllerOwner.targeting.target;
        ownerRigidbody = ownerGameObject.GetComponent<Rigidbody2D>();
        enemyAttack = ownerGameObject.GetComponent<EnemyAttack>();
        grounder = ownerGameObject.GetComponent<EnemyGrounder>();
        wallCheck = ownerGameObject.GetComponent<EnemyWallChecker>();
        timer = 0;
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
            if (Vector2.Distance(ownerGameObject.transform.position, target.position) <= enemyAttack.attackRange)
            {
                enemyAttack.IsInRange = true;
                if (timer >= enemyAttack.attackSpeed)
                {
                    enemyAttack.Attack(target);

                    timer = 0;
                }
            }
            else
            {
                enemyAttack.IsInRange = false;
                controllerOwner.stateMachine.ChangeState(new ChaseState(ownerGameObject));
            }
        }
        else
        {
            controllerOwner.stateMachine.ChangeState(new MoveState(ownerGameObject));
        }
    }
}
