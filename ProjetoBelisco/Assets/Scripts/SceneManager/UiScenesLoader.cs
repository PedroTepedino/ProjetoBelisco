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

        private static void ActivateScene(AsyncOperation obj)
        {
            obj.allowSceneActivation = true;
        }
    }
}


