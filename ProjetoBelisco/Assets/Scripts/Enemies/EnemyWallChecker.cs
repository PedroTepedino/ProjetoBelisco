using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyWallChecker : MonoBehaviour
{
    private EnemyController controller;

    [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _checkerCenter = Vector3.zero;
    [FoldoutGroup("Parameters")] [SerializeField] private float _checkerSizes;
    [FoldoutGroup("Parameters")] [SerializeField] [EnumToggleButtons] private LayerMask _wallLayerMask;
        
    public bool wallAhead { get; private set; } = false;

    private void Start() {
        controller = GetComponent<EnemyController>();
        _checkerCenter = this.transform.position + controller.groundDetectionOffset;
    }
    private void Update()
    {
        wallAhead = WallCheck();
    }
    
    private bool WallCheck()
    {
        RaycastHit2D raycastHit2D;
        _checkerCenter = this.transform.position + new Vector3(controller.movingRight ? controller.groundDetectionOffset.x : -controller.groundDetectionOffset.x , controller.groundDetectionOffset.y, 0);
        raycastHit2D = Physics2D.Raycast(_checkerCenter, Vector2.right, _checkerSizes, _wallLayerMask);
        /*if(controller.movingRight){
            _checkerCenter = this.transform.position + new Vector3(controller.groundDetectionOffset.x, controller.groundDetectionOffset.y, controller.groundDetectionOffset.z);
            raycastHit2D = Physics2D.Raycast(_checkerCenter, Vector2.right, _checkerSizes, _wallLayerMask);
        }else{
            _checkerCenter = this.transform.position + new Vector3(-controller.groundDetectionOffset.x, controller.groundDetectionOffset.y, controller.groundDetectionOffset.z);
            raycastHit2D = Physics2D.Raycast(_checkerCenter, Vector2.left, _checkerSizes, _wallLayerMask);
        }*/
        return raycastHit2D.collider != null ? true : false;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = wallAhead ? Color.red : Color.green;

        Gizmos.DrawLine(_checkerCenter, _checkerCenter+new Vector3(_checkerSizes, controller.groundDetectionOffset.y, 0))
    }
}
