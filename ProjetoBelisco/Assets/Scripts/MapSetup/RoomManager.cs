using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Belisco
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] [InlineEditor(InlineEditorObjectFieldModes.Boxed)] private RoomParameters _roomParameters;

        [SerializeField] private RoomSpawner _initialCheckpoint;
        
        private IEnumerator OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                yield return UnloadConnections();
                
                yield return LoadConnections();
            }
        }

        private IEnumerator LoadConnections()
        {
            List<AsyncOperation> operations = new List<AsyncOperation>();
            foreach (var connections in _roomParameters.SceneConnections)
            {
                var con = connections.TryLoadScene();
                if (con != null)
                    operations.Add(con);
            }
            yield return new WaitUntil(()=>operations.All(op => op.isDone));
        }
        
        private IEnumerator UnloadConnections()
        {
            List<AsyncOperation> operations = new List<AsyncOperation>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (_roomParameters.SceneConnections.All(scn => scn.ThisSceneAsset.name != scene.name) 
                    && scene.name != "MapSetup" && scene.name != _roomParameters.ThisSceneAsset.name)
                {
                    operations.Add(SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects));
                }
            }
            
            yield return new WaitUntil(()=>operations.All(op => op.isDone));
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

            if (_initialCheckpoint == null)
            {
                var roomSpawner = this.GetComponentInChildren<RoomSpawner>();
                
                if (roomSpawner != null)
                    _initialCheckpoint = roomSpawner;
                else
                {
                    _initialCheckpoint = RoomSpawner.CreateSpawner();
                    _initialCheckpoint.transform.parent = this.transform;
                }
            }
        }
    }
}