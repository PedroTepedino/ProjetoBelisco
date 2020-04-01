using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyController
{
    public override void Update()
    {
        RaycastHit2D hitObjects = Physics2D.Raycast(this.transform.position, Vector2.right, lookingRange, LayerMask.GetMask("Player"));
        Debug.Log(hitObjects);
        if (hitObjects.collider != null)
        {
            target = hitObjects.transform;
            if(actualState != "attack")
            {
                this.stateMachine.ChangeState(new AttackState(this.gameObject, this.GetComponent<EnemyController>(), target));
            }
        }
        this.stateMachine.RunStateUpdate();
        if (Vector2.Distance(this.transform.position, new Vector2(target.position.x, target.position.y)) > lookingRange )
        {
            target = null;
        }
    }
    
    public override void EnemyDie()
    {
        Debug.Log("ded");
    }
}
