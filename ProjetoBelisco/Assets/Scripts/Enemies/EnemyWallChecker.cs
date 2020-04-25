using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyWallChecker : MonoBehaviour
{
    private EnemyController controller;

    [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _wallCheckerTop = Vector3.zero;
    [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _wallCheckerCenter = Vector3.zero;
    [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _wallCheckerBottom = Vector3.zero;
    [FoldoutGroup("Parameters")] [SerializeField] private float _checkerSizes;
    [FoldoutGroup("Parameters")] [SerializeField] [EnumToggleButtons] private LayerMask _wallLayerMask;
        
    private Vector3 _checkerTop;
    private Vector3 _checkerCenter;
    private Vector3 _checkerBottom;

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
        _checkerTop = this.transform.position + new Vector3(controller.movingRight ? _wallCheckerTop.x : -_wallCheckerTop.x, _wallCheckerTop.y, 0);
        raycastHit2DTop = Physics2D.Raycast(_checkerTop, controller.movingRight ? Vector2.right : Vector2.left, _checkerSizes, _wallLayerMask);
        wallTop = raycastHit2DTop.collider != null ? true : false;

        RaycastHit2D raycastHit2DCenter;
        _checkerCenter = this.transform.position + new Vector3(controller.movingRight ? _wallCheckerCenter.x : -_wallCheckerCenter.x, _wallCheckerCenter.y, 0);
        raycastHit2DCenter = Physics2D.Raycast(_checkerCenter, controller.movingRight ? Vector2.right : Vector2.left, _checkerSizes, _wallLayerMask);
        wallCenter = raycastHit2DCenter.collider != null ? true : false;

        RaycastHit2D raycastHit2DBottom;
        _checkerBottom = this.transform.position + new Vector3(controller.movingRight ? _wallCheckerBottom.x : -_wallCheckerBottom.x, _wallCheckerBottom.y, 0);
        raycastHit2DBottom = Physics2D.Raycast(_checkerBottom, controller.movingRight ? Vector2.right : Vector2.left, _checkerSizes, _wallLayerMask);
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
        // Guard sentence
        if (controller == null)
            return;
        
        Gizmos.color = wallAhead ? Color.red : Color.green;

        if(controller.movingRight){
            Gizmos.DrawLine(this.transform.position + _wallCheckerTop, this.transform.position + new Vector3(_wallCheckerTop.x + _checkerSizes, _wallCheckerTop.y, 0));
            Gizmos.DrawLine(this.transform.position + _wallCheckerCenter, this.transform.position + new Vector3(_wallCheckerCenter.x + _checkerSizes, _wallCheckerCenter.y, 0));
            Gizmos.DrawLine(this.transform.position + _wallCheckerBottom, this.transform.position + new Vector3(_wallCheckerBottom.x + _checkerSizes, _wallCheckerBottom.y, 0));

        }
        else
        {
            Gizmos.DrawLine(this.transform.position + new Vector3(-_wallCheckerTop.x, _wallCheckerTop.y, 0), this.transform.position + new Vector3(-_wallCheckerTop.x - _checkerSizes, _wallCheckerTop.y, 0));
            Gizmos.DrawLine(this.transform.position + new Vector3(-_wallCheckerCenter.x, _wallCheckerCenter.y, 0), this.transform.position + new Vector3(-_wallCheckerCenter.x - _checkerSizes, _wallCheckerCenter.y, 0));
            Gizmos.DrawLine(this.transform.position + new Vector3(-_wallCheckerBottom.x, _wallCheckerBottom.y, 0), this.transform.position + new Vector3(-_wallCheckerBottom.x - _checkerSizes, _wallCheckerBottom.y, 0));  
        }
    }
}
