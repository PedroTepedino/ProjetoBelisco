using Cinemachine;
using GameScripts.Player;
using UnityEngine;

namespace GameScripts.Camera
{
    public class CameraManager : MonoBehaviour
    {
        CinemachineVirtualCamera cam;

        private void Awake()
        {
            cam = this.GetComponent<CinemachineVirtualCamera>();
            Life.OnPlayerSpawn += SetCameraToPlayer;
        }

        private void OnDestroy()
        {
            Life.OnPlayerSpawn -= SetCameraToPlayer;
        }

        private void SetCameraToPlayer(GameObject player)
        {
            cam.Follow = player.transform;
        }
    }
}
