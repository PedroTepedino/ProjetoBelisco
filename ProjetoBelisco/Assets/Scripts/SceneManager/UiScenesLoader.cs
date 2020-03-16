using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScenesIndex;
using Sirenix.OdinInspector;
using System;

public class UiScenesLoader : MonoBehaviour
{
    [SerializeField] [EnumToggleButtons] private UiScenes _uiSceneToLoad;

    private void Awake()
    {
        LoadScene();    
    }

    private void LoadScene()
    {
        SceneManager.LoadSceneAsync((int)_uiSceneToLoad, LoadSceneMode.Additive).completed += ActivateScene;
    }

    private void ActivateScene(AsyncOperation obj)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)_uiSceneToLoad));
    }
}
