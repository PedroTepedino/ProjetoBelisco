using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyLife))]
[RequireComponent(typeof(EnemyWallChecker))]
[RequireComponent(typeof(EnemyGrounder))]
/*Class: EnemyController
 * Abstract Class that describes the essential components of every enemy.
 */
public abstract class EnemyController : MonoBehaviour
{
    public StateMachine stateMachine;
    public Rigidbody2D rigidbody;
    public Transform target;
    public string actualState;
    public bool movingRight = true;
   
    [EnumPaging]public LayerMask layerTargeting;
    [EnumPaging]public LayerMask layerTarget;
    public float movingSpeed = 5;
    public float lookingRange = 5;
    public float attackRange = 1;
    public float attackSpeed = 1;
    public int attackDamage = 1;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        stateMachine = gameObject.AddComponent<StateMachine>();
        this.stateMachine.ChangeState(new MoveState(this.gameObject));
    }
    public abstract void Update();

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;

        if (movingRight)
        {
            Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(lookingRange,0 , 0));
            Gizmos.DrawLine(this.transform.position+ new Vector3(0, 0.5f, 0), this.transform.position + new Vector3(attackRange, 0.5f, 0));            
        }
        else
        {
            Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-lookingRange,0 , 0));
            Gizmos.DrawLine(this.transform.position+ new Vector3(0, 0.5f, 0), this.transform.position + new Vector3(-attackRange, 0.5f, 0));
        }
    }
    
}
