#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using GameScripts.Environment;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace GameScripts.Tools.Editor
{
    public class CheckFirstSpawner : OdinEditorWindow
    {
        [ShowInInspector] [PropertyOrder(10)] private int _totalSpawnerCount = -1;
        [ShowInInspector] [PropertyOrder(11)] private int _playerSpawnerCount = -1;

        [ShowInInspector] 
        [ListDrawerSettings(Expanded = true,HideAddButton = true,IsReadOnly = true, ShowIndexLabels = true,ShowItemCount = true, DraggableItems = false)]
        [ReadOnly]
        [PropertyOrder(12)]
        private List<PlayerRespawner> _playerSpawners;
    
        [ShowInInspector] 
        [ListDrawerSettings(Expanded = true,HideAddButton = true,IsReadOnly = true, ShowIndexLabels = true,ShowItemCount = true, DraggableItems = false)]
        [ReadOnly]
        [PropertyOrder(13)]
        private List<PlayerRespawner> _checkPoints;

        [SerializeField]
        [PropertyOrder(9)]
        private GameObject _checkpointPrefab;
    
        [SerializeField]
        [PropertyOrder(9)]
        private GameObject _playerSpawnerPrefab;
    
        private bool ToFewPlayerSpawners => _playerSpawnerCount < 1;
        private bool ToManyPlayerSpawners => _playerSpawnerCount > 1;
        private bool ToFewCheckpoints => _totalSpawnerCount < 1;

        [MenuItem("Tools/Check For First Spawner", priority = -10000)]
        private static void OpenWindow()
        {
            GetWindow<CheckFirstSpawner>().Show();
        }
    
        protected override void Initialize()
        {    
            _checkpointPrefab = Resources.Load("Prefabs/Player/Checkpoint") as GameObject;
            _playerSpawnerPrefab = Resources.Load("Prefabs/Player/PlayerSpawner") as GameObject;
            Check();
        }

        protected override void OnBeginDrawEditors()
        {
            if (_totalSpawnerCount < 1)
            {
                SirenixEditorGUI.ErrorMessageBox("Insufficient Spawns");
            }
            else
            {
                if (_playerSpawnerCount <= 0)
                {
                    SirenixEditorGUI.ErrorMessageBox("Need at Least one player first spawner on the scene");
                }
                else if (_playerSpawnerCount > 1)
                {
                    SirenixEditorGUI.ErrorMessageBox("Cannot Have more than one player First Spawner per scene!!");
                }   
            }   
        }
    
        [Button(ButtonSizes.Large)] 
        [PropertyOrder(0)]
        public void Check()
        {
            _playerSpawners = new List<PlayerRespawner>();
            _checkPoints = new List<PlayerRespawner>(FindObjectsOfType<PlayerRespawner>());
            _checkPoints.Reverse();
        
            _totalSpawnerCount = _checkPoints.Count;
        
            _playerSpawnerCount = 0;
            foreach (PlayerRespawner playerSpawner in _checkPoints?.Where(playerSpawner => playerSpawner.IsFirstSpawner))
            {
                _playerSpawnerCount++;
                _playerSpawners.Add(playerSpawner);
            }
        }

        [Button(ButtonSizes.Large)]
        public void SelectAllSpawners()
        {
            if (_playerSpawnerCount < 1) return;
        
            List<Object> aux = new List<Object>();
        
            foreach (PlayerRespawner VARIABLE in _playerSpawners)
            {
                aux.Add(VARIABLE.gameObject);
            }
        
            Selection.objects = aux.ToArray();
        }

        [Button(ButtonSizes.Large)]
        public void SelectAllCheckpoints()
        {
            if (_totalSpawnerCount < 1) return;
        
            List<Object> aux = new List<Object>();
        
            foreach (var VARIABLE in _checkPoints)
            {
                aux.Add(VARIABLE.gameObject);
            
            }

            Selection.objects = aux.ToArray();
        }

        [ShowIf("ToFewPlayerSpawners")]
        [Button(ButtonSizes.Large)]
        public void AddPlayerSpawnerFromExisting()
        {
            if (ToFewCheckpoints)
            {
                CreatePlayerSpawner();
            }
        
            Check();
        }
    
        public enum RemoveType { all,  leaveOne}

        [ShowIf("ToManyPlayerSpawners")]
        [Button(ButtonSizes.Large, Expanded = true, Style = ButtonStyle.CompactBox, Name = "Remove Player Spawners")]
        public void RemovePlayerSpawnersFromExisting([EnumToggleButtons]RemoveType type = RemoveType.all)
        {
            if (type == RemoveType.all)
            {
                foreach (var VARIABLE in _playerSpawners)
                {
                    VARIABLE.IsFirstSpawner = false;
                }
            }
            else
            {
                for (int i = 1; i < _playerSpawners.Count; i++)
                {
                    _playerSpawners[i].IsFirstSpawner = false;
                }
            }
        
            Check();
        }

        [ShowIf("ToManyPlayerSpawners")]
        [Button(ButtonSizes.Large)]
        public void DeleteExtraSpawners()
        {
            foreach (PlayerRespawner t in _playerSpawners)
            {
                if (t.IsFirstSpawner == true)
                {
                    DestroyImmediate(t);
                }
            }
            Check();
        }

        [ShowIf("ToFewPlayerSpawners")]
        [Button(ButtonSizes.Large)]
        public void CreatePlayerSpawner()
        {
            ((GameObject) PrefabUtility.InstantiatePrefab(_playerSpawnerPrefab)).GetComponent<PlayerRespawner>()
                .IsFirstSpawner = true;
            Check();
        }

        [Button(ButtonSizes.Large)]
        public void CreateCheckpoint()
        {
            ((GameObject) PrefabUtility.InstantiatePrefab(_checkpointPrefab)).GetComponent<PlayerRespawner>()
                .IsFirstSpawner = false;
            Check();
        }
    }
}

#endif