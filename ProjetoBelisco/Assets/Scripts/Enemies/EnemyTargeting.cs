using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyTargeting : MonoBehaviour
{
    [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _targetingCenter;
    [FoldoutGroup("Parameters")] [SerializeField] [EnumToggleButtons] private LayerMask _targetingLayerMask;

    private EnemyController controller;
    private Vector3 _checkerCenter;

    public Transform target { get; private set; } = null;
    public bool hasTarget { get; private set; } = false;

    void Start()
    {
        controller = GetComponent<EnemyController>();
    }

    void Update()
    {
        hasTarget = Targeting();
    }

    private bool Targeting(){
        var hitObject = Physics2D.Raycast(this.transform.position, controller.movingRight ? Vector2.right : Vector2.left, controller.lookingRange, _targetingLayerMask);
        if (hitObject.collider != null)
        {
            if (hitObject.transform.gameObject.GetComponent<PlayerLife>() != null)
            {
                target = hitObject.transform;
                return true;
            }else
            {
                target = null;
                return false;
            }
        }else
        {
            target = null;
            return false;
        }
    }

    protected void OnDrawGizmos() {
        // Guard sentence
        if (controller == null)
            return;
        
        Gizmos.color = Color.blue;

        if (controller.movingRight)
        {
            Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(controller.lookingRange,0 , 0));
            Gizmos.DrawLine(this.transform.position+ new Vector3(0, 0.5f, 0), this.transform.position + new Vector3(controller.attackRange, 0.5f, 0));            
        }
        else
        {
            Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(-controller.lookingRange,0 , 0));
            Gizmos.DrawLine(this.transform.position+ new Vector3(0, 0.5f, 0), this.transform.position + new Vector3(-controller.attackRange, 0.5f, 0));
        }
    }
}
