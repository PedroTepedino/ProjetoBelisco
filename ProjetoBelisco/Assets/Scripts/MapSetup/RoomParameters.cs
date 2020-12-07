using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Belisco
{
    [CreateAssetMenu(menuName = "Room Parameter", fileName = "NewRoomParameter")]
    public class RoomParameters : ScriptableObject
    {
        [SerializeField] [AssetSelector] [AssetsOnly] private RoomParameters[] _sceneConnections;

        [SerializeField] private SceneAsset _thisSceneAsset;
        [SerializeField] private string _thisScenePath;

        public SceneAsset ThisSceneAsset => _thisSceneAsset;
        public string ThisScenePath => _thisScenePath;

        public RoomParameters[] SceneConnections => _sceneConnections;
        

        public AsyncOperation TryLoadScene()
        {
            if (SceneManager.GetSceneByName(_thisSceneAsset.name).isLoaded) return null;

            return SceneManager.LoadSceneAsync(_thisSceneAsset.name, LoadSceneMode.Additive);
        }
        

        public void UnloadScene()
        {
            var scene = SceneManager.GetSceneByName(ThisSceneAsset.name);
            if (scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            }
        }

        private void Init(SceneAsset scene)
        {
            _thisSceneAsset = scene;
        }

        public void InitAtPath(string scenePath)
        {
            Init(AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath));
            _thisScenePath = scenePath;
        }
    }
    
#if UNITY_EDITOR
    public static class RoomManagerFactory
    {
        public static RoomParameters CreateRoomParameter(string name)
        {
            RoomParameters asset = ScriptableObject.CreateInstance<RoomParameters>();
            
            AssetDatabase.CreateAsset(asset, $"Assets/ParametersObjects/MAPS/ROOM_MANAGER_{name}.asset");
            AssetDatabase.SaveAssets();

            return asset;
        }
    }
#endif
}