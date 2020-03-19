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
    public float speed;
    public Transform groundDetection;
    public bool movingRight = true;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    protected override void Die()
    {
        throw new System.NotImplementedException();
    }


    void Update()
    {
        
    }
}
