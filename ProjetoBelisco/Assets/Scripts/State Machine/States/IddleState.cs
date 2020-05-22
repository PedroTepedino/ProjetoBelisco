using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IddleState : IState
{
    private GameObject ownerGameObject;
    private EnemyController controllerOwner;

    public IddleState(GameObject owner)
    {
        ownerGameObject = owner;
        controllerOwner = owner.GetComponent<EnemyController>();  
    }

    public void EnterState()
    {
        controllerOwner.actualState = "iddle";
    }

    public void ExitState()
    {
        
    }

    public void RunState()
    {
        
    }

}
