using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BasicEnemy : EnemyController
{    
    public override void Update()
    {
        var hitObjects = Physics2D.Raycast(this.transform.position, Vector2.right, lookingRange, layerTargeting);
        
        if (hitObjects.collider != null)
        {
            if(actualState != "attack")
            {
                this.stateMachine.ChangeState(new AttackState(this.gameObject, this.GetComponent<EnemyController>(), hitObjects.transform));
            }
        }
        this.stateMachine.RunStateUpdate();
    }
}
