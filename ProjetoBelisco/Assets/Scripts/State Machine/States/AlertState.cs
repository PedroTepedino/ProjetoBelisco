using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IState
{

    private GameObject ownerGameObject;
    private Transform target;
    private EnemyController controllerOwner;
    private float timer;
    private float alertAnimationTime;

    public AlertState(GameObject owner)
    {
        ownerGameObject = owner;
        controllerOwner = owner.GetComponent<EnemyController>();       
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
                controllerOwner.stateMachine.ChangeState(new ChaseState(ownerGameObject));
            }
        }
        else
        {
            controllerOwner.stateMachine.ChangeState(new MoveState(ownerGameObject));
        }
    }
}
