using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyGrounder : MonoBehaviour
{
    [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _grounderCenter;
    [FoldoutGroup("Parameters")] [SerializeField] private float _grounderSize;
    [FoldoutGroup("Parameters")] [SerializeField] [EnumToggleButtons] private LayerMask _groundLayerMask;
   
    private EnemyController controller;
    private Vector3 _checkerCenter;
        
    public bool isGrounded { get; private set; } = false;


    private void Start() {
        controller = GetComponent<EnemyController>();
    }

    private void Update()
    {
        isGrounded = GroundCheck();
    }
    
    private bool GroundCheck()
    {
        RaycastHit2D raycastHit2D;
        _checkerCenter = this.transform.position + new Vector3(controller.movingRight ? _grounderCenter.x : -_grounderCenter.x , _grounderCenter.y, 0);
        raycastHit2D = Physics2D.Raycast(_checkerCenter, Vector2.down, _grounderSize, _groundLayerMask);

        return raycastHit2D.collider != null;
    }

    protected void OnDrawGizmos()
    {
        // Guard sentence
        if (controller == null)
            return;
        
        Gizmos.color = isGrounded ? Color.green : Color.red;
        if(controller.movingRight){
            Gizmos.DrawLine(this.transform.position + _grounderCenter, this.transform.position + new Vector3(_grounderCenter.x, _grounderCenter.y - _grounderSize, 0));
        }else{
            Gizmos.DrawLine(this.transform.position + new Vector3(-_grounderCenter.x, _grounderCenter.y , 0), this.transform.position + new Vector3(-_grounderCenter.x, _grounderCenter.y - _grounderSize, 0));
        }
    }
}
