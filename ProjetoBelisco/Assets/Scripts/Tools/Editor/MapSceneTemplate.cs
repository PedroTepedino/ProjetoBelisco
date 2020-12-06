#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Belisco
{
    public class MapSceneTemplate : OdinEditorWindow
    {
        private string[] _biomeNames;
        private List<BiomeType> _biomes;

        [SerializeField] [EnumToggleButtons] [HideLabel] private Interction _interction = Interction.load;
        private enum Interction
        {
            load,
            create,
        }

        [MenuItem("Window/Room Scenes Manager", priority = -1000)]
        private static void OpenWindow()
        {
            var window = GetWindow<MapSceneTemplate>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
            window.Show();
        }

        protected override void Initialize()
        {
            UpdateBiomesList();
        }

        private void UpdateBiomesList()
        {
            _biomeNames = AssetDatabase.GetSubFolders("Assets/Scenes/Maps");

            if (_biomes == null)
            {
                _biomes = new List<BiomeType>();
            }
            else
            {
                _biomes.Clear();
            }

            for (var index = 0; index < _biomeNames.Length; index++)
            {
                var biomeName = _biomeNames[index];
                _biomes.Add(new BiomeType(biomeName));
            }
        }

        private string _newBiomeErrorMessage = null;
        
        [Button(ButtonSizes.Gigantic, ButtonStyle.FoldoutButton)]
        public void CreateNewBiome(string biomeName, bool createFirstRoom = true)
        {
            _newBiomeErrorMessage = null;
            
            if (biomeName == null)
            {
                _newBiomeErrorMessage = "Biome name can not be null!";
                return;
            }

            biomeName = biomeName.ToUpper();

            if (AssetDatabase.IsValidFolder($"Assets/Scenes/Maps/{biomeName}"))
            {
                _newBiomeErrorMessage = "Biome Already Exists!";
                return;
            }

            AssetDatabase.CreateFolder("Assets/Scenes/Maps", biomeName);
            AssetDatabase.SaveAssets();
            
            UpdateBiomesList();

            if (createFirstRoom)
            {
                CreateRoom(_biomes.Find(biome => biome.Name == biomeName));
            }
        }

        protected override void OnBeginDrawEditors()
        {
            if (_biomeNames == null || _biomeNames.Length == 0)
            {
                SirenixEditorGUI.WarningMessageBox("No Biomes Have Been Created");
            }

            if (_biomes != null)
            {
                GUILayout.Space(10);
                var labelStyle = new GUIStyle();
                labelStyle.margin = new RectOffset(5, 5, 5, 2);
                labelStyle.fontSize = 20;
                labelStyle.normal.textColor = Color.white;
                labelStyle.fontStyle = FontStyle.Bold;
                GUILayout.Label("Click On of the Buttons to create a new Room", labelStyle);
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    var style = new GUIStyle(GUI.skin.button);
                    style.margin = new RectOffset(10, 10, 10, 10);
                    style.fixedHeight = 100f;
                    style.stretchWidth = true;

                    for (int i = 0; i <= _biomes.Count / 3; i++)
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            for (int j = 0; j < 3 && (i * 3) + j < _biomes.Count; j++)
                            {
                                if (GUILayout.Button(_biomes[(i * 3) + j].Name, style))
                                {
                                    switch (_interction)
                                    {
                                        case Interction.create:
                                            CreateRoom(_biomes[(i * 3) + j]);
                                            break;
                                        case Interction.load:
                                            LoadAllScenes(_biomes[(i * 3) + j]);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void LoadAllScenes(BiomeType biome)
        {
            var scenes = AssetDatabase.FindAssets("t:Scene", new[] {"Assets/Scenes/Maps/" + biome.Name});

            for (int i = 0; i < EditorSceneManager.sceneCount; i++)
            {
                if (EditorSceneManager.GetSceneAt(i).name != "MapSetup" && !EditorSceneManager.GetSceneAt(i).name.Contains(biome.Name))
                {
                    EditorSceneManager.UnloadSceneAsync(EditorSceneManager.GetSceneAt(i),
                        UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                }
            }

            foreach (var scene in scenes)
            {
                EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(scene), OpenSceneMode.Additive);
            }

            if (EditorSceneManager.GetActiveScene().name != "MapSetup" && !EditorSceneManager.GetActiveScene().name.Contains(biome.Name))
            {
                EditorSceneManager.UnloadSceneAsync(EditorSceneManager.GetActiveScene(),
                    UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            }

            if (!EditorSceneManager.GetSceneByName("MapSetup").IsValid())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/GamplaySetup/MapSetup.unity", OpenSceneMode.Additive);
            }
        }

        private void CreateRoom(BiomeType biomeType)
        {
            var path = GetScenePath(biomeType.Name);

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);

            var sceneName = path.Replace("Assets/Scenes/Maps/" + biomeType.Name + "/", "");
            sceneName = sceneName.Replace(".unity", "");
            scene.name = sceneName;

            CreateObjects(sceneName);

            EditorSceneManager.SaveScene(scene, path);
        }

        private void CreateObjects(string sceneName)
        {
            // var gridGameObject = new GameObject("Grid");
            // gridGameObject.layer = LayerMask.NameToLayer("Ground");
            // gridGameObject.AddComponent<Grid>();
            //
            // var tilemap = new GameObject(sceneName);
            // tilemap.layer = LayerMask.NameToLayer("Ground");
            // tilemap.transform.parent = gridGameObject.transform;
            //
            // tilemap.AddComponent<Tilemap>();
            // tilemap.AddComponent<TilemapRenderer>();
            // tilemap.AddComponent<SceneNameObserver>();
            //
            // var tilemapCollider2D = tilemap.AddComponent<TilemapCollider2D>();
            // tilemapCollider2D.usedByComposite = true;
            //
            // var rigidBody2D = tilemap.AddComponent<Rigidbody2D>();
            // rigidBody2D.bodyType = RigidbodyType2D.Static;
            //
            // var compositeCollider2D = tilemap.AddComponent<CompositeCollider2D>();
            // compositeCollider2D.sharedMaterial =
            //     AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>("Assets/PhysicsMaterial/Ground.physicsMaterial2D");
            
            var grid = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GameplaySetup/Grid.prefab")) as GameObject;
            grid.transform.position = new Vector3(0,0,0);
            
            // var playerCam = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GameplaySetup/PlayerCam.prefab")) as GameObject;
            // playerCam.transform.position = new Vector3(0,0,-10);
            
            var roomManager = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GameplaySetup/[RoomManager].prefab")) as GameObject;
            roomManager.transform.position = new Vector3(0,0,0);

            var parametersAux = RoomManagerFactory.CreateRoomParameter(sceneName);
            
        }

        private static string GetScenePath(string biome)
        {
            var scenes = AssetDatabase.FindAssets("t:Scene", new[] {"Assets/Scenes/Maps/" + biome});

            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = AssetDatabase.GUIDToAssetPath(scenes[i]);
            }

            for (int i = 0; i < scenes.Length; i++)
            {
                if (!scenes.Contains($"Assets/Scenes/Maps/{biome}/{biome}_ROOM_{i + 1}.unity"))
                {
                    return $"Assets/Scenes/Maps/{biome}/{biome}_ROOM_{i + 1}.unity";
                }
            }

            if (AssetDatabase.FindAssets($"Assets/Scenes/Maps/{biome}/{biome}_ROOM_{scenes.Length + 1}.unity").Length > 0)
            {
                return AssetDatabase.GenerateUniqueAssetPath($"Assets/Scenes/Maps/{biome}/{biome}_ROOM_{scenes.Length + 1}.unity");
            }
            
            return $"Assets/Scenes/Maps/{biome}/{biome}_ROOM_{scenes.Length + 1}.unity";
        }

        protected override void OnEndDrawEditors()
        {
            if (_newBiomeErrorMessage != null)
            {
                SirenixEditorGUI.ErrorMessageBox(_newBiomeErrorMessage);
            }
        }

        [System.Serializable]
        public class BiomeType
        {
            public readonly string Name;
            public readonly string Path;

            public BiomeType(string biomeName)
            {
                Name = biomeName.Replace("Assets/Scenes/Maps/", "");
                Path = biomeName;
            }
        }
    }
}
#endif