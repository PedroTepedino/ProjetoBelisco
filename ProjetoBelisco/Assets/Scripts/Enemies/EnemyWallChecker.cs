using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyWallChecker : MonoBehaviour
{
    private EnemyController controller;

    [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _checkerTop = Vector3.zero;
    [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _checkerCenter = Vector3.zero;
    [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _checkerBottom = Vector3.zero;
    [FoldoutGroup("Parameters")] [SerializeField] private float _checkerSizes;
    [FoldoutGroup("Parameters")] [SerializeField] [EnumToggleButtons] private LayerMask _wallLayerMask;
        
    public bool wallAhead { get; private set; } = false;
    public bool wallTop = false;
    public bool wallCenter = false;
    public bool wallBottom = false;

    private void Start() {
        controller = GetComponent<EnemyController>();
    }
    private void Update()
    {
        wallAhead = WallCheck();
    }
    
    private bool WallCheck()
    {
        RaycastHit2D raycastHit2DTop;
        _checkerTop = this.transform.position + new Vector3(controller.movingRight ? _checkerTop.x : -_checkerTop.x , _checkerTop.y, 0);
        raycastHit2DTop = Physics2D.Raycast(_checkerTop, Vector2.right, _checkerSizes, _wallLayerMask);
        wallTop = raycastHit2DTop.collider != null ? true : false;

        RaycastHit2D raycastHit2DCenter;
        _checkerCenter = this.transform.position + new Vector3(controller.movingRight ? _checkerCenter.x : -_checkerCenter.x , _checkerCenter.y, 0);
        raycastHit2DCenter = Physics2D.Raycast(_checkerCenter, Vector2.right, _checkerSizes, _wallLayerMask);
        wallCenter = raycastHit2DCenter.collider != null ? true : false;

        RaycastHit2D raycastHit2DBottom;
        _checkerBottom = this.transform.position + new Vector3(controller.movingRight ? _checkerBottom.x : -_checkerBottom.x , _checkerBottom.y, 0);
        raycastHit2DBottom = Physics2D.Raycast(_checkerBottom, Vector2.right, _checkerSizes, _wallLayerMask);
        wallBottom = raycastHit2DBottom.collider != null ? true : false;

        /*if(controller.movingRight){
            _checkerCenter = this.transform.position + new Vector3(controller.groundDetectionOffset.x, controller.groundDetectionOffset.y, controller.groundDetectionOffset.z);
            raycastHit2D = Physics2D.Raycast(_checkerCenter, Vector2.right, _checkerSizes, _wallLayerMask);
        }else{
            _checkerCenter = this.transform.position + new Vector3(-controller.groundDetectionOffset.x, controller.groundDetectionOffset.y, controller.groundDetectionOffset.z);
            raycastHit2D = Physics2D.Raycast(_checkerCenter, Vector2.left, _checkerSizes, _wallLayerMask);
        }*/
        
        if(!wallTop && !wallCenter && !wallBottom){
            wallAhead = false;
        }
        else
        {
            wallAhead = true;
        }

        return wallAhead;

    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = wallAhead ? Color.red : Color.green;

        Gizmos.DrawLine(this.transform.position + _checkerTop, this.transform.position + new Vector3(_checkerTop.x + _checkerSizes, _checkerTop.y, 0));
        Gizmos.DrawLine(this.transform.position + _checkerCenter, this.transform.position + new Vector3(_checkerCenter.x + _checkerSizes, _checkerCenter.y, 0));
        Gizmos.DrawLine(this.transform.position + _checkerBottom, this.transform.position + new Vector3(_checkerBottom.x + _checkerSizes, _checkerBottom.y, 0));
    }
}
