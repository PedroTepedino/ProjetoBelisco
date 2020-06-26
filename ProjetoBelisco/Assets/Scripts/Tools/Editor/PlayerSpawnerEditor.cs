#if UNITY_EDITOR
using GameScripts.Environment;
using UnityEditor;
using UnityEngine;

namespace GameScripts.Tools.Editor
{
    [InitializeOnLoad()]

    public class PlayerSpawnerEditor : MonoBehaviour
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmos(PlayerRespawner spawn, GizmoType gizmoType)
        {
            Color color = Color.white;
            if ((gizmoType & GizmoType.Selected) != 0)
            {
                color = Color.yellow;
            }

            if (PlayerRespawner.CurrentSpawner == spawn)
            {
                color = Color.red;
            }

            string gizmo = "CheckPoint.png";
            if (spawn.IsFirstSpawner)
            {
                gizmo = "PlayerSpawner.png";
            }

            Gizmos.DrawIcon(spawn.transform.position, gizmo, true, color);
        }
    }
}
#endif
