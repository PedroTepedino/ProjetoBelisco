using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyLife))]
[RequireComponent(typeof(EnemyWallChecker))]
[RequireComponent(typeof(EnemyGrounder))]
/*Class: EnemyController
 * Abstract Class that inherits from <LifeSystemAbstract> that describes the essential components of every enemy.
 */
public abstract class EnemyController : MonoBehaviour
{
    public StateMachine stateMachine;
    public Rigidbody2D rigidbody;
    public Transform target;
    public string actualState;
    public bool movingRight = true;
   
    [EnumPaging]public LayerMask layerTargeting;
    [EnumPaging]public LayerMask layerMove;
    //public Vector3 groundDetectionOffset;
    public float movingSpeed;
    public float lookingRange;
    public float attackRange;
    public float attackSpeed;
    public int attackDamage;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
        this.stateMachine.ChangeState(new MoveState(this.gameObject, this.GetComponent<EnemyController>()));
    }
    public abstract void Update();
    
}
