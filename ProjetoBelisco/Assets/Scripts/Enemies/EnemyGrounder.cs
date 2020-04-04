using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyGrounder : MonoBehaviour
{
    [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _grounderCenter = Vector3.zero;
    [FoldoutGroup("Parameters")] [SerializeField] private Vector2 _grounderSizes;
    [FoldoutGroup("Parameters")] [SerializeField] [EnumToggleButtons] private LayerMask _grounderLayerMask;
        
    public static bool IsGrounded { get; private set; } = false;

    private void Update()
    {
        IsGrounded = GroundCheck();
    }
    
    private bool GroundCheck()
    {
        return Physics2D.OverlapBox(this.transform.position + _grounderCenter, _grounderSizes, 0f, _grounderLayerMask) != null ? true : false;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded ? Color.red : Color.green;

        Gizmos.DrawWireCube(this.transform.position + _grounderCenter, _grounderSizes);
    }
}
