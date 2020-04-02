using UnityEngine;
using UnityEngine.SceneManagement;
using ScenesIndex;
using UnityEngine.Experimental.Rendering.Universal;

public class UiScenesLoader : MonoBehaviour
{
    public static Camera MainCamera { get; set; }
    public static PixelPerfectCamera PixelPerfectCamera { get; set; }

    private void Awake()
    {
        MainCamera = Camera.main;
        if (MainCamera != null) PixelPerfectCamera = MainCamera.gameObject.GetComponent<PixelPerfectCamera>();
    }

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
