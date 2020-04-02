using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BasicEnemy : EnemyController
{
    [SerializeField] [EnumToggleButtons] private LayerMask _layerMask;
    
    public override void Update()
    {
        var hitObjects = Physics2D.Raycast(this.transform.position, Vector2.right, lookingRange, _layerMask);
        
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
