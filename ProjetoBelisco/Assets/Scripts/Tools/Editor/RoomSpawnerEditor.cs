using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

namespace Belisco
{
    [InitializeOnLoad]
    public class RoomSpawnerEditor : MonoBehaviour
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmos(RoomSpawner spawner, GizmoType gizmoType)
        {
            Color color = Color.white;
            if ((gizmoType & GizmoType.Selected) != 0) color = Color.yellow;
            
            var gizmo = "SpawnIcon.tiff";
            
            Gizmos.DrawIcon(spawner.transform.position, gizmo, true, color);
        }
    }
}