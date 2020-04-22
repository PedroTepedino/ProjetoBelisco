using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private GameObject ownerGameObject;
    private Transform _target;
    private EnemyController controllerOwner;
    private Rigidbody2D ownerRigidbody;
    private EnemyGrounder grounder;
    private EnemyWallChecker wallCheck;
    private PlayerLife targerLifeSystem;
    private float timer;

    public AttackState(GameObject owner, Transform target)
    {
        ownerGameObject = owner;
        controllerOwner = owner.GetComponent<EnemyController>();
        _target = target;       
    }

    public void EnterState()
    {
        Debug.Log(_target);
        controllerOwner.actualState = "attack";
        ownerRigidbody = ownerGameObject.GetComponent<Rigidbody2D>();
        targerLifeSystem = _target.GetComponentInParent<PlayerLife>();
        grounder = ownerGameObject.GetComponent<EnemyGrounder>();
        wallCheck = ownerGameObject.GetComponent<EnemyWallChecker>();
        timer = 0;
    }

    public void ExitState()
    {

    }

    public void RunState()
    {
        timer += Time.deltaTime;

        if(_target != null)
        {
            if (Vector2.Distance(ownerGameObject.transform.position, _target.position) <= controllerOwner.lookingRange)
            {
                if (Vector2.Distance(ownerGameObject.transform.position, _target.position) <= controllerOwner.attackRange)
                {
                    if (timer >= controllerOwner.attackSpeed)
                    {
                        Debug.Log("dmg");
                        targerLifeSystem.Damage(controllerOwner.attackDamage);

                        timer = 0;
                    }
                }
                else
                {
                    Vector2 direction = (_target.position - ownerGameObject.transform.position).normalized;
                    if (grounder.isGrounded && wallCheck.wallAhead)
                    {
                        ownerRigidbody.velocity = direction * controllerOwner.movingSpeed;
                    }
                    else
                    {
                        controllerOwner.target = null;
                        controllerOwner.stateMachine.ChangeState(new MoveState(ownerGameObject));
                    }
                }
            }
            else
            {
                controllerOwner.target = null;
                controllerOwner.stateMachine.ChangeState(new MoveState(ownerGameObject));
            }
        }
        else
        {
            controllerOwner.target = null;
             controllerOwner.stateMachine.ChangeState(new MoveState(ownerGameObject));
        }
    }
}
