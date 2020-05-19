using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BasicEnemy : EnemyController
{    
    public override void Update()
    {
        if (targeting.hasTarget)
        {
            if (actualState != "alert" && actualState != "attack")
            {
                this.stateMachine.ChangeState(new AlertState(this.gameObject));
            }
            
        }
        this.stateMachine.RunStateUpdate();
    }
}
