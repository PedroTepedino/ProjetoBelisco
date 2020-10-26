﻿using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Belisco
{
    public class SceneEssentials : OdinMenuEditorWindow
    {
        private CreateNewSceneEssentials _createNewSceneEssentials;

        private List<GameObject> _missingGameObjects;

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_createNewSceneEssentials != null) DestroyImmediate(_createNewSceneEssentials.EssentialObjects);
        }

        [MenuItem("Tools/Scene Essentials", priority = -10000)]
        private static void OpenWindow()
        {
            GetWindow<SceneEssentials>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree();

            _createNewSceneEssentials = new CreateNewSceneEssentials();

            tree.Add("Create New Scene Type", _createNewSceneEssentials);
            tree.AddAllAssetsAtPath("Scene Essentials", "Assets/Editor/Tools/SceneEssentials",
                typeof(SceneEssentialObjects));

            return tree;
        }


        protected override void OnBeginDrawEditors()
        {
            OdinMenuTreeSelection selected = MenuTree.Selection;

            if (selected.SelectedValue != null)
            {
                SirenixEditorGUI.BeginHorizontalToolbar();

                GUILayout.Label(MenuTree.Selection.FirstOrDefault().Name);

                if (selected.SelectedValue.GetType() == typeof(SceneEssentialObjects))
                {
                    GUILayout.FlexibleSpace();
                    if (SirenixEditorGUI.ToolbarButton("Delete Current"))
                    {
                        SceneEssentialObjects asset = selected.SelectedValue as SceneEssentialObjects;
                        var path = AssetDatabase.GetAssetPath(asset);
                        AssetDatabase.DeleteAsset(path);
                        AssetDatabase.SaveAssets();
                    }
                }

                SirenixEditorGUI.EndHorizontalToolbar();
            }
        }

        protected override void OnEndDrawEditors()
        {
            OdinMenuTreeSelection selected = MenuTree.Selection;

            if (selected.SelectedValue != null)
                if (selected.SelectedValue.GetType() == typeof(SceneEssentialObjects))
                {
                    SceneEssentialObjects asset = selected.SelectedValue as SceneEssentialObjects;

                    if (_missingGameObjects != null)
                    {
                        foreach (GameObject obj in _missingGameObjects)
                            SirenixEditorGUI.WarningMessageBox("Missing -" + obj.name + "- Prefab.");
                    }
                    else
                    {
                        if (asset.HasBeenChecked)
                            SirenixEditorGUI.InfoMessageBox("No missing Objects.");
                        else
                            SirenixEditorGUI.InfoMessageBox("Not Checked Yet.");
                    }

                    GUILayout.FlexibleSpace();
                    SirenixEditorGUI.BeginHorizontalToolbar();
                    {
                        GUILayout.FlexibleSpace();
                        if (_missingGameObjects != null)
                            if (SirenixEditorGUI.ToolbarButton("Add Missing Objects"))
                                AddMissing();

                        if (SirenixEditorGUI.ToolbarButton("Check Scene"))
                        {
                            CheckScene(asset);
                            asset.HasBeenChecked = true;
                        }
                    }
                    SirenixEditorGUI.EndHorizontalToolbar();
                }
        }

        private void AddMissing()
        {
            foreach (GameObject obj in _missingGameObjects)
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
            var objectsInScene = FindObjectsOfType<GameObject>();

            if (_missingGameObjects != null) _missingGameObjects = null;

            _missingGameObjects = new List<GameObject>();

            foreach (GameObject assetObj in asset.Prefabs)
            {
                var objectExists = false;
                foreach (GameObject sceneObj in objectsInScene)
                    if (PrefabUtility.GetCorrespondingObjectFromSource(sceneObj) == assetObj)
                        objectExists = true;

                if (!objectExists) _missingGameObjects.Add(assetObj);
            }

            if (_missingGameObjects.Count == 0) _missingGameObjects = null;
        }

        public class CreateNewSceneEssentials
        {
            [LabelText("Essentials")]
            [Tooltip("The Objectes that are essetial to this type os scene")]
            [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
            public SceneEssentialObjects EssentialObjects;

            [LabelText("Scene Type: ")]
            [SuffixLabel("Name", true)]
            [Required("Name for the Scene Type is Required!")]
            [ValidateInput("ValidateName", "Must Have a unique name.")]
            public string EssentialsName;

            public CreateNewSceneEssentials()
            {
                EssentialObjects = CreateInstance<SceneEssentialObjects>();
                EssentialObjects.Prefabs = new List<GameObject>();
                EssentialsName = null;
            }

            [Button("Save", ButtonSizes.Large)]
            private void CreateNewSceneType()
            {
                AssetDatabase.CreateAsset(EssentialObjects,
                    "Assets/Editor/Tools/SceneEssentials/" + EssentialsName + ".asset");
                AssetDatabase.SaveAssets();

                // Create new Instance of the Scriptable Object
                EssentialObjects = CreateInstance<SceneEssentialObjects>();
                EssentialObjects.Prefabs = new List<GameObject>();
                EssentialsName = null;
            }

            private bool ValidateName(string name)
            {
                if (name == null)
                {
                    return false;
                }

                var sceneTypes = Resources.LoadAll<SceneEssentialObjects>("Tools/SceneEssentials");

                foreach (SceneEssentialObjects st in sceneTypes)
                    if (name.ToLower() == st.name.ToLower())
                        return false;
                return true;
            }
        }
    }
}