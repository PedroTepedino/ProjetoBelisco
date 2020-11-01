using UnityEditor;
using UnityEngine;

namespace Belisco
{
    [InitializeOnLoad]
    public class RoomTransitionSpawnerEditor : MonoBehaviour
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmos(RoomTransitionSpawner spawner, GizmoType gizmoType)
        {
            Color color = Color.white;
            if ((gizmoType & GizmoType.Selected) != 0) color = Color.green;
            
            var gizmo = "Transitions.tiff";
            
            Gizmos.DrawIcon(spawner.transform.position, gizmo, true, color);
        }
    }
}