using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "SceneEssentials", menuName = "Tools/SceneEssentials", order = 1)]
public class SceneEssentialObjects : ScriptableObject
{
    public bool HasBeenChecked { get; set; } = false;

    [AssetsOnly]
    public List<GameObject> Prefabs;
}
