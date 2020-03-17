using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScenesIndex;
using Sirenix.OdinInspector;
using System;

public class UiScenesLoader : MonoBehaviour
{
    public static void LoadScene(UiScenes sceneToLoad)
    {
        SceneManager.LoadSceneAsync((int)sceneToLoad, LoadSceneMode.Additive).completed += ActivateScene;
    }

    private static void ActivateScene(AsyncOperation obj)
    {
        obj.allowSceneActivation = true;
    }

    public static void UnLoadScene(UiScenes sceneToUnload)
    {
        SceneManager.UnloadSceneAsync((int)sceneToUnload);
    }
}
