using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private GameObject ownerGameObject;
    private Transform _target;
    private EnemyController controllerOwner;
    private float timer;

    public AttackState(GameObject owner, Transform target, EnemyController controller)
    {
        ownerGameObject = owner;
        _target = target;
        controllerOwner = controller;
        timer = 0;
    }

    public void EnterState()
    {
        
    }

    public void ExitState()
    {
        
    }

    public void RunState()
    {
        timer += Time.deltaTime;

        ownerGameObject.transform.LookAt(_target);
        if (Vector2.Distance(ownerGameObject.transform.position, _target.position) <= controller.attackRange)
        {
            if (timer >= controller.attackSpeed)
            {
                if (_target != null)
                {
                    _target.GetComponent<LifeManager>().Damage(controller.attackDamage);
                }
                timer = 0;
            }
        }
        else
        {
            Debug.Log("move");
            _navMeshAgent.SetDestination(_target.position);
        }
    }
}
