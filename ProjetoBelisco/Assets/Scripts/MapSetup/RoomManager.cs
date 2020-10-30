using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Belisco
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] [InlineEditor(InlineEditorObjectFieldModes.Boxed)] private RoomParameters _roomParameters;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                UnloadConnections();
                
                LoadConnections();
            }
        }

        private void LoadConnections()
        {
            foreach (var connections in _roomParameters.SceneConnections)
            {
                connections.TryLoadScene();
            }
        }

        private void UnloadConnections()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (_roomParameters.SceneConnections.All(scn => scn.ThisSceneAsset.name != scene.name) 
                    && scene.name != "MapSetup")
                {
                    SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                }
            }
        }

        private void OnValidate()
        {
            this.name = $"[ROOM_MANAGER_{this.gameObject.scene.name}]";

            if (_roomParameters == null)
            {
               _roomParameters = AssetDatabase.LoadAssetAtPath<RoomParameters>(
                    $"Assets/ParametersObjects/MAPS/ROOM_MANAGER_{this.gameObject.scene.name}.asset");
            }
            
            if (_roomParameters != null && (_roomParameters.ThisSceneAsset == null || _roomParameters.ThisSceneAsset.SafeIsUnityNull()))
            {
                _roomParameters.Init(AssetDatabase.LoadAssetAtPath<SceneAsset>(this.gameObject.scene.path));
            }
        }
        
    }
}