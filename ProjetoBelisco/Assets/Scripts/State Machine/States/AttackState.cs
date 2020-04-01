using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private GameObject ownerGameObject;
    private Transform _target;
    private EnemyController controllerOwner;
    private float timer;
    private LifeSystemAbstract targerLifeSystem;
    private Rigidbody2D ownerRigidbody;

    public AttackState(GameObject owner, EnemyController controller, Transform target)
    {
        ownerGameObject = owner;
        controllerOwner = controller;
        _target = target;

    }

    public void EnterState()
    {
        controllerOwner.actualState = "attack";
        ownerRigidbody = ownerGameObject.GetComponent<Rigidbody2D>();
        targerLifeSystem = _target.GetComponent<LifeSystemAbstract>();
        timer = 0;
    }

    public void ExitState()
    {

    }

    public void RunState()
    {
        timer += Time.deltaTime;

        //ownerGameObject.transform.LookAt(_target);
        if (Vector2.Distance(ownerGameObject.transform.position, _target.position) <= controllerOwner.lookingRange)
        {
            if (Vector2.Distance(ownerGameObject.transform.position, _target.position) <= controllerOwner.attackRange)
            {
                if (timer >= controllerOwner.attackSpeed)
                {
                    if (_target != null)
                    {
                        Debug.Log("dmg");
                        //targerLifeSystem.Damage(controllerOwner.attackDamage);
                    }
                    timer = 0;
                }
            }
            else
            {
                Vector2 direction = (_target.position - ownerGameObject.transform.position).normalized;
                ownerRigidbody.velocity = direction * controllerOwner.movingSpeed;
            }
        }
        else
        {
            controllerOwner.stateMachine.ChangeState(new MoveState(ownerGameObject, controllerOwner));
            controllerOwner.actualState = "move";
        }
    }
}
