using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    [CreateAssetMenu(fileName = "SceneEssentials", menuName = "Tools/SceneEssentials", order = 1)]
    public class SceneEssentialObjects : ScriptableObject
    {
        [AssetsOnly] public List<GameObject> Prefabs;

        public bool HasBeenChecked { get; set; } = false;
    }
}