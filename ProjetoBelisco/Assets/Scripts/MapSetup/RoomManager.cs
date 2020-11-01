﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
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

        [SerializeField] private RoomSpawner _initialCheckpoint;

        //[SerializeField] private List<RoomTransitionSpawner> _roomTransitions;

        private static CinemachineBrain _mainCam = null;

        private void Awake()
        {
            if (_mainCam == null)
            {
                _mainCam = FindObjectOfType<CinemachineBrain>();
            }
        }

        private IEnumerator OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<Player>();
                
                yield return UnloadConnections();
                
                yield return LoadConnections();

                Time.timeScale = 0;

                // Tween moveAnimation = null;
                // if (player.CurrentRoomManager != null)
                // {
                //     var spawner = this._roomTransitions.Find(room =>
                //         room.PreviousRoom == player.CurrentRoomManager._roomParameters);
                //     if (spawner != null)
                //     {
                //         Debug.Log($"{spawner} { _mainCam.ActiveBlend}");
                //         moveAnimation = player.transform.DOMove(spawner.transform.position, _mainCam.ActiveBlend?.Duration ?? 0.1f)
                //             .SetEase(Ease.Linear).SetAutoKill(true).SetRecyclable(true).SetUpdate(UpdateType.Normal, true);
                //         moveAnimation.Play();
                //         //player.transform.position = spawner.transform.position;
                //     }
                // }

                yield return new WaitWhile(()=>_mainCam.IsBlending);

                //moveAnimation?.Kill();

                Time.timeScale = 1;

                player.CurrentRoomManager = this;
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
            else
            {
                if (_roomParameters.ThisSceneAsset == null || _roomParameters.ThisSceneAsset.SafeIsUnityNull())
                {
                    _roomParameters.Init(AssetDatabase.LoadAssetAtPath<SceneAsset>(this.gameObject.scene.path));
                }

                // if (_roomTransitions != null && _roomTransitions.Count > 0)
                // {
                //     _roomTransitions.RemoveAll(room => room == null);
                // }
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