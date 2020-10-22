using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace GameScripts.Tools.Editor
{
    public class SceneEssentials : OdinMenuEditorWindow
    {
        private CreateNewSceneEssentials _createNewSceneEssentials;

        private List<GameObject> _missingGameObjects;

        [MenuItem("Tools/Scene Essentials", priority = -10000)]
        private static void OpenWindow()
        {
            GetWindow<SceneEssentials>().Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_createNewSceneEssentials != null)
            {
                DestroyImmediate(_createNewSceneEssentials.EssentialObjects);
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();

            _createNewSceneEssentials = new CreateNewSceneEssentials();

            tree.Add("Create New Scene Type", _createNewSceneEssentials);
            tree.AddAllAssetsAtPath("Scene Essentials", "Assets/Editor/Tools/SceneEssentials", typeof(SceneEssentialObjects));
        
            return tree;
        }
    

        protected override void OnBeginDrawEditors()
        {   
            OdinMenuTreeSelection selected = this.MenuTree.Selection;

            if (selected.SelectedValue != null)
            {
                SirenixEditorGUI.BeginHorizontalToolbar();

                GUILayout.Label(this.MenuTree.Selection.FirstOrDefault().Name);

                if (selected.SelectedValue.GetType() == typeof(SceneEssentialObjects))
                {
                    {
                        GUILayout.FlexibleSpace();
                        if (SirenixEditorGUI.ToolbarButton("Delete Current"))
                        {
                            SceneEssentialObjects asset = selected.SelectedValue as SceneEssentialObjects;
                            string path = AssetDatabase.GetAssetPath(asset);
                            AssetDatabase.DeleteAsset(path);
                            AssetDatabase.SaveAssets();
                        }
                    }
                
                }
                SirenixEditorGUI.EndHorizontalToolbar();
            }
        }

        protected override void OnEndDrawEditors()
        {
            OdinMenuTreeSelection selected = this.MenuTree.Selection;

            if (selected.SelectedValue != null)
            {
                if (selected.SelectedValue.GetType() == typeof(SceneEssentialObjects))
                {
                    SceneEssentialObjects asset = selected.SelectedValue as SceneEssentialObjects;

                    if (_missingGameObjects != null)
                    {
                        foreach (GameObject obj in _missingGameObjects)
                        {
                            SirenixEditorGUI.WarningMessageBox("Missing -" + obj.name + "- Prefab.");
                        }
                    }
                    else
                    {
                        if (asset.HasBeenChecked)
                        {
                            SirenixEditorGUI.InfoMessageBox("No missing Objects.");
                        }
                        else
                        {
                            SirenixEditorGUI.InfoMessageBox("Not Checked Yet.");
                        }
                    }

                    GUILayout.FlexibleSpace();
                    SirenixEditorGUI.BeginHorizontalToolbar();
                    {
                        GUILayout.FlexibleSpace();
                        if (_missingGameObjects !=  null)
                        {
                            if (SirenixEditorGUI.ToolbarButton("Add Missing Objects"))
                            {
                                AddMissing();
                            }
                        }

                        if (SirenixEditorGUI.ToolbarButton("Check Scene"))
                        {       
                            CheckScene(asset);
                            asset.HasBeenChecked = true;
                        }
                    }
                    SirenixEditorGUI.EndHorizontalToolbar();
                }
            }
        }

        private void AddMissing()
        {
            foreach(GameObject obj in _missingGameObjects)
            {
                GameObject aux = PrefabUtility.InstantiatePrefab(obj) as GameObject;
                aux.transform.position = new Vector3(0f, 0f, -10f);
                aux.transform.rotation = Quaternion.identity;
            }

            _missingGameObjects.Clear();
            _missingGameObjects = null;
        }

        private void CheckScene(SceneEssentialObjects asset)
        {
            GameObject[] objectsInScene = Object.FindObjectsOfType<GameObject>();

            if (_missingGameObjects != null)
            {
                _missingGameObjects = null;
            }

            _missingGameObjects = new List<GameObject>();

            foreach (GameObject assetObj in asset.Prefabs)
            {
                bool objectExists = false;
                foreach (GameObject sceneObj in objectsInScene)
                {
                    if (PrefabUtility.GetCorrespondingObjectFromSource(sceneObj) == assetObj)
                    {
                        objectExists = true;
                    }
                }            

                if (!objectExists)
                {
                    _missingGameObjects.Add(assetObj);
                }
            }

            if (_missingGameObjects.Count == 0)
            {
                _missingGameObjects = null;
            }
        }

        public class CreateNewSceneEssentials
        {
            public CreateNewSceneEssentials()
            {
                EssentialObjects = ScriptableObject.CreateInstance<SceneEssentialObjects>();
                EssentialObjects.Prefabs = new List<GameObject>();
                EssentialsName = null;
            }

            [LabelText("Scene Type: ")]
            [SuffixLabel("Name", overlay: true)]
            [Required("Name for the Scene Type is Required!")]
            [ValidateInput("ValidateName", "Must Have a unique name.", InfoMessageType.Error)]
            public string EssentialsName;

            [LabelText("Essentials")]
            [Tooltip("The Objectes that are essetial to this type os scene")]
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public SceneEssentialObjects EssentialObjects;

            [Button("Save", ButtonSizes.Large)]
            private void CreateNewSceneType()
            {
                AssetDatabase.CreateAsset(EssentialObjects, "Assets/Resources/Tools/SceneEssentials/" + EssentialsName + ".asset");
                AssetDatabase.SaveAssets();

                // Create new Instance of the Scriptable Object
                EssentialObjects = ScriptableObject.CreateInstance<SceneEssentialObjects>();
                EssentialObjects.Prefabs = new List<GameObject>();
                EssentialsName = null;
            }

            private bool ValidateName(string name)
            {
                if (name == null)
                {
                    return false;
                }
                else
                {
                    SceneEssentialObjects[] sceneTypes = Resources.LoadAll<SceneEssentialObjects>("Tools/SceneEssentials");

                    foreach (SceneEssentialObjects st in sceneTypes)
                    {
                        if (name.ToLower() == st.name.ToLower())
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
