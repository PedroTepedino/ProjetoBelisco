using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Linq;

public class CollectableSpawnerPlacer : OdinEditorWindow
{
    [HorizontalGroup("Top")]

    [AssetsOnly]
    [AssetSelector]
    [SerializeField]
    private GameObject _spawnerPrefab;

    [HorizontalGroup("Middle")]

    [ShowInInspector]
    [VerticalGroup("Middle/Left")]
    private int _collectableCount = 0;

    [ShowInInspector]
    [VerticalGroup("Middle/Left")]
    private int _spawnersCount = 0;

    [MenuItem("Tools/Collectable Spwaner Placer", priority = -10000)]
    private static void OpenWindow()
    {
        GetWindow<CollectableSpawnerPlacer>().Show();
    }

    protected override void Initialize()
    {
        _spawnerPrefab = Resources.Load("Prefabs/Collectables/Spawner/CollectableSpawner") as GameObject;
        CountCollectablesAndSpawners();
    }

    [HorizontalGroup("Middle")]
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

        foreach (BaseCollectableObject obj in collectables)
        {
            GameObject aux = PrefabUtility.InstantiatePrefab(_spawnerPrefab) as GameObject;
            aux.transform.position = obj.transform.position;
            aux.GetComponent<CollectableSpawner>().CollectableType = obj.Type;

            DestroyImmediate(obj.gameObject);
        }

        CountCollectablesAndSpawners();
    }
}
