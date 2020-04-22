using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BasicEnemy : EnemyController
{    
    public override void Update()
    {
        var hitObjects = Physics2D.Raycast(this.transform.position, movingRight ? Vector2.right : Vector2.left, lookingRange, layerTargeting);
        
        if (hitObjects.collider != null)
        {
            Debug.Log("hit");
            if (hitObjects.transform.gameObject.CompareTag("Player"))
            {   
                Debug.Log("player");
                target = hitObjects.transform;
                if(actualState != "attack")
                {
                    this.stateMachine.ChangeState(new AttackState(this.gameObject, target));
                }               
            }
        }
        this.stateMachine.RunStateUpdate();
    }
}
