using UnityEngine;
using ScenesIndex;

public class PlayerUiManager : MonoBehaviour
{
    private void Awake()
    {
        UiScenesLoader.LoadScene(UiScenes.PlayerUi);
        UiScenesLoader.LoadScene(UiScenes.PawsCounter);
    }
}
