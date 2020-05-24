using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace GameScripts.SceneManager
{
    public class UiScenesLoader : MonoBehaviour
    {
        public static UnityEngine.Camera MainCamera { get; set; }
        public static PixelPerfectCamera PixelPerfectCamera { get; set; }

        private void Awake()
        {
            MainCamera = UnityEngine.Camera.main;
            if (MainCamera != null) PixelPerfectCamera = MainCamera.gameObject.GetComponent<PixelPerfectCamera>();
        }

        public static void LoadScene(UiScenes sceneToLoad)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)sceneToLoad, LoadSceneMode.Additive).completed += ActivateScene;
        }

        private static void ActivateScene(AsyncOperation obj)
        {
            obj.allowSceneActivation = true;
        }

        public static void UnLoadScene(UiScenes sceneToUnload)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)sceneToUnload);
        }
    }
}


