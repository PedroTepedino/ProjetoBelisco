using Cinemachine;
using UnityEngine;

namespace Belisco
{
    public class CameraManager : MonoBehaviour
    {
        private CinemachineVirtualCamera cam;

        private void Awake()
        {
            cam = GetComponent<CinemachineVirtualCamera>();
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