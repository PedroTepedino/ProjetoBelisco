using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Belisco
{
    public class CollectableSpawnerPlacer : OdinEditorWindow
    {
        [HorizontalGroup("Top")] [AssetsOnly] [AssetSelector] [SerializeField]
        private GameObject _spawnerPrefab;

        [HorizontalGroup("Middle")] [ShowInInspector] [VerticalGroup("Middle/Left")]
        private int _collectableCount;

        [ShowInInspector] [VerticalGroup("Middle/Left")]
        private int _spawnersCount;

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
            //_collectableCount = FindObjectsOfType<BaseObject>().Length;
            //_spawnersCount = FindObjectsOfType<Spawner>().Length;
        }

        [Button(ButtonSizes.Large, Name = "Put Spawners")]
        public void ReplaceCollectables()
        {
            //BaseObject[] collectables = FindObjectsOfType<BaseObject>();

            // foreach (BaseObject obj in collectables)
            // {
            //     GameObject aux = PrefabUtility.InstantiatePrefab(_spawnerPrefab) as GameObject;
            //     aux.transform.position = obj.transform.position;
            //     aux.GetComponent<Spawner>().CollectableType = obj.Type;
            //
            //     DestroyImmediate(obj.gameObject);
            // }

            CountCollectablesAndSpawners();
        }
    }
}