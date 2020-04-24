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
    private PlayerLife targerLifeSystem;
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
        targerLifeSystem = target.GetComponentInParent<PlayerLife>();
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
            Debug.Log("has target");
            if (Vector2.Distance(ownerGameObject.transform.position, target.position) <= controllerOwner.attackRange)
            {
                Debug.Log("has target");
                if (timer >= controllerOwner.attackSpeed)
                {
                    Debug.Log("dmg");
                    targerLifeSystem.Damage(controllerOwner.attackDamage);

                    timer = 0;
                }
            }
            else
            {
                Debug.Log("chase");
                controllerOwner.stateMachine.ChangeState(new ChaseState(ownerGameObject));
            }
        }
        else
        {
            Debug.Log("move");
            controllerOwner.stateMachine.ChangeState(new MoveState(ownerGameObject));
        }
    }
}
