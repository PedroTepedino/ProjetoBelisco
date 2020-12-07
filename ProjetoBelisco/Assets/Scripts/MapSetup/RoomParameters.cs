using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Belisco
{
    [CreateAssetMenu(menuName = "Room Parameter", fileName = "NewRoomParameter")]
    public class RoomParameters : ScriptableObject
    {
        [SerializeField] [AssetSelector] [AssetsOnly]
        private RoomParameters[] _sceneConnections;

        [SerializeField] private string _thisSceneName;
        [SerializeField] private string _thisScenePath;

        public string ThisSceneName => _thisSceneName;

        public string ThisScenePath => _thisScenePath;

        public RoomParameters[] SceneConnections => _sceneConnections;


        public AsyncOperation TryLoadScene()
        {
            if (SceneManager.GetSceneByName(_thisSceneName).isLoaded) return null;

            Debug.Log(_thisSceneName + " " + this.name);

            return SceneManager.LoadSceneAsync(_thisSceneName, LoadSceneMode.Additive);
        }


        public void UnloadScene()
        {
            var scene = SceneManager.GetSceneByName(ThisSceneName);
            if (scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            }
        }

#if UNITY_EDITOR
        private void Init(SceneAsset scene)
        {
            _thisSceneName = scene.name;
        }


        public void InitAtPath(string scenePath)
        {
            Init(AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath));
            _thisScenePath = scenePath;
        }

        public bool IsInitiated()
        {
            return _thisScenePath != null && _thisSceneName != null;
        }
#endif
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