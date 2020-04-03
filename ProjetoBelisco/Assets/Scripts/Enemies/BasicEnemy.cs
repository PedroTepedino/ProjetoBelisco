using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BasicEnemy : EnemyController
{
    

    public override void Update()
    {
        RaycastHit2D hitObjects = Physics2D.Raycast(this.transform.position, Vector2.right, lookingRange, layerTargeting);
        if (hitObjects.collider != null)
        {
            Debug.Log(hitObjects.collider.gameObject.name, hitObjects.collider.gameObject);
            if (hitObjects.transform.GetComponentInParent<PlayerLife>() != null)
            {
                target = hitObjects.transform;
                if (actualState != "attack")
                {
                    this.stateMachine.ChangeState(new AttackState(this.gameObject, this.GetComponent<EnemyController>(), target));
                }
            }
        }
        else
        {
            Debug.Log("null target");
        }
        if (target != null)
        {
            if (Vector2.Distance(this.transform.position, new Vector2(target.position.x, target.position.y)) > lookingRange)
            {
                target = null;
            }
        }
        this.stateMachine.RunStateUpdate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(this.transform.position, (this.transform.position + (Vector3.right * lookingRange)));
        Gizmos.DrawLine((this.transform.position + (Vector3.down * 0.3f)), (this.transform.position + new Vector3(1.1f, -0.3f, 0)));
    }
    public override void EnemyDie()
    {
        Debug.Log("ded");
    }
}
