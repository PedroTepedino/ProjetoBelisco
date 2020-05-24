using GameScripts.Collectables;
using UnityEditor;
using UnityEngine;

namespace GameScripts.Tools.Editor
{
    [InitializeOnLoad()]
    public class CollectableSpawnerEditor
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable )]
        public static void OnDrawSceneGizmos(Spawner spawn, GizmoType gizmoType)
        {
            Color color = Color.white;
            if ((gizmoType & GizmoType.Selected) != 0)
            {
                color = Color.yellow;
            }

            Gizmos.DrawIcon(spawn.transform.position, "Spawner.png", true, color);
        }
    }
}
