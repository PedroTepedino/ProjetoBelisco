using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyController
{
    public override void Update()
    {
        this.stateMachine.RunStateUpdate();
    }
    
    public override void EnemyDie()
    {

    }

}
