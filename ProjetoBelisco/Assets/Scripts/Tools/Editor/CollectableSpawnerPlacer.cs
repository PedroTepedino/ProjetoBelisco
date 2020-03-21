using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Linq;

public class CollectableSpawnerPlacer : OdinEditorWindow
{
    [HorizontalGroup("Top")]

    [ShowInInspector]
    [VerticalGroup("Top/Left")]
    private int _collectableCount = 0;

    [ShowInInspector]
    [VerticalGroup("Top/Left")]
    private int _spawnersCount = 0;

    [MenuItem("Tools/Collectable Spwaner Placer", priority = -10000)]
    private static void OpenWindow()
    {
        GetWindow<CollectableSpawnerPlacer>().Show();
    }

    protected override void Initialize()
    {
        CountCollectablesAndSpawners();
    }

    [HorizontalGroup("Top")]
    [Button(ButtonSizes.Large, Name = "Count")]
    public void CountCollectablesAndSpawners()
    {
        _collectableCount = FindObjectsOfType<BaseCollectableObject>().Length;
        _spawnersCount = FindObjectsOfType<CollectableSpawner>().Length;
    }

    [Button(ButtonSizes.Large, Name = "Put Spawners")]
    public void ReplaceCollectables()
    {
        BaseCollectableObject[] collectables = FindObjectsOfType<BaseCollectableObject>();
    }
}
