using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyLife))]
[RequireComponent(typeof(EnemyWallChecker))]
[RequireComponent(typeof(EnemyGrounder))]
[RequireComponent(typeof(EnemyTargeting))]
[RequireComponent(typeof(EnemyAttack))]
/*Class: EnemyController
 * Abstract Class that describes the essential components of every enemy.
 */
public abstract class EnemyController : MonoBehaviour
{
    [EnumPaging]public LayerMask layerTargeting;
    public StateMachine stateMachine;
    public Rigidbody2D rigidbody;
    public EnemyTargeting targeting;
    public string actualState;
    public bool movingRight = true;
    public float movingSpeed = 5;
    public float lookingRange = 5;
    public float alertAnimationTime = 0;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        stateMachine = gameObject.AddComponent<StateMachine>();
        this.stateMachine.ChangeState(new MoveState(this.gameObject));
        targeting = GetComponent<EnemyTargeting>();
    }
    public abstract void Update();
}
