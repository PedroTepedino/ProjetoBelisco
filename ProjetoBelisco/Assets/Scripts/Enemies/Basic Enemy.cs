using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyController
{
    public override void Update()
    {
        var hitObjects = Physics2D.Raycast(this.transform.position, Vector2.right, lookingRange, LayerMask.GetMask("Player"));
        if (hitObjects != null)
        {
            if(stateMachine.GetCurrentState() != null/*AttackState*/)
            {
                this.stateMachine.ChangeState(new AttackState(this.gameObject, this.GetComponent<EnemyController>(), hitObjects.transform));
                actualState = "attack";
            }
        }
        this.stateMachine.RunStateUpdate();
    }
    
    public override void EnemyDie()
    {
        Debug.Log("ded");
    }

}
