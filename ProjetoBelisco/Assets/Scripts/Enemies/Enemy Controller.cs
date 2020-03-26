using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
/*Class: EnemyController
 * Abstract Class that inherits from <LifeSystemAbstract> that describes the essential components of every enemy.
 */
public abstract class EnemyController : LifeSystemAbstract
{
    public Rigidbody2D rigidbody;
    public float movingSpeed;
    public Transform groundDetection;
    public bool movingRight = true;
    public StateMachine stateMachine = new StateMachine();
    public float attackRange;
    public float attackDamage;
    public float attackSpeed;
    public float lookingRange;
    public string actualState;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        this.stateMachine.ChangeState(new MoveState(this.gameObject, this.GetComponent<EnemyController>()));
        actualState = "move";
    }
    protected override void Die()
    {
        EnemyDie();
    }


    public abstract void EnemyDie();
    public abstract void Update();
    
}
