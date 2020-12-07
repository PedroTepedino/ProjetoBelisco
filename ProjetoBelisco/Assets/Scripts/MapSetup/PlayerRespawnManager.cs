using UnityEngine;

namespace Belisco
{
    public class PlayerRespawnManager : MonoBehaviour
    {
        private void OnEnable()
        {
            Player.OnPlayerDeath += ListenOnPlayerDeath;
        }

        private void OnDisable()
        {
            Player.OnPlayerDeath -= ListenOnPlayerDeath;
        }

        private void ListenOnPlayerDeath()
        {
            Debug.Log(Player.CurrentRoomManager);
            if (Player.CurrentRoomManager != null)
            {
                LoadLevel.LevelToLoad = Player.CurrentRoomManager.RoomParameters.ThisSceneAsset.name;
            }
        }
    }
}