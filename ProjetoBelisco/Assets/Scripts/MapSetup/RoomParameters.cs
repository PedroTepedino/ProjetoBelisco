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

        public SceneAsset ThisSceneAsset => _thisSceneAsset;

        public RoomParameters[] SceneConnections => _sceneConnections;


        public AsyncOperation TryLoadScene()
        {
            if (SceneManager.GetSceneByName(_thisSceneAsset.name).isLoaded) return null;
            
            return SceneManager.LoadSceneAsync(ThisSceneAsset.name, LoadSceneMode.Additive);
        }
        

        public void UnloadScene()
        {
            var scene = SceneManager.GetSceneByName(ThisSceneAsset.name);
            if (scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            }
        }

        public void Init(SceneAsset scene)
        {
            _thisSceneAsset = scene;
        }
    }
    
#if UNITY_EDITOR
    public static class RoomManagerFactory
    {
        public static void CreateRoomParameter(string name)
        {
            RoomParameters asset = ScriptableObject.CreateInstance<RoomParameters>();
            
            AssetDatabase.CreateAsset(asset, $"Assets/ParametersObjects/MAPS/ROOM_MANAGER_{name}.asset");
            AssetDatabase.SaveAssets();
        }
    }
#endif
}