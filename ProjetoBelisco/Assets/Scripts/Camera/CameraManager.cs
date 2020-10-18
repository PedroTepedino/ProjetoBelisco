using Cinemachine;
using GameScripts.Player;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class CameraManager : MonoBehaviour
    {
        CinemachineVirtualCamera cam;

        private void Awake()
        {
            cam = this.GetComponent<CinemachineVirtualCamera>();
            Player.OnPlayerSpawn += SetCameraToPlayer;
        }

        private void OnDestroy()
        {
            Player.OnPlayerSpawn -= SetCameraToPlayer;
        }

        private void SetCameraToPlayer(GameObject player)
        {
            cam.Follow = player.transform;
        }
    }
}
