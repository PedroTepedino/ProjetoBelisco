using GameScripts.SceneManager;
using UnityEngine;

namespace GameScripts.Ui
{
    public class PlayerUiManager : MonoBehaviour
    {
        private void Awake()
        {
            UiScenesLoader.LoadScene(UiScenes.PlayerUi);
            UiScenesLoader.LoadScene(UiScenes.PawsCounter);
        }
    }
}
