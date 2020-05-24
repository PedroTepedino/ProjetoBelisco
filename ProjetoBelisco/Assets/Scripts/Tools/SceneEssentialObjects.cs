using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameScripts.Tools
{
    [CreateAssetMenu(fileName = "SceneEssentials", menuName = "Tools/SceneEssentials", order = 1)]
    public class SceneEssentialObjects : ScriptableObject
    {
        public bool HasBeenChecked { get; set; } = false;

        [AssetsOnly]
        public List<GameObject> Prefabs;
    }
}
