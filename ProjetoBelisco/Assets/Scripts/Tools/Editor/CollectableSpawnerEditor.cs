using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class CollectableSpawnerEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(CollectableSpawner spawn, GizmoType gizmoType)
    {
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.green * 0.5f;
        }

        Gizmos.DrawSphere(spawn.transform.position, 0.5f);
    }
  
}
