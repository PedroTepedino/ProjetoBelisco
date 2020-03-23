using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    CinemachineVirtualCamera cam;

    private void Awake()
    {
        cam = this.GetComponent<CinemachineVirtualCamera>();
        PlayerLife.OnPlayerSpawn += SetCameraToPlayer;
    }

    private void OnDestroy()
    {
        PlayerLife.OnPlayerSpawn -= SetCameraToPlayer;
    }

    private void SetCameraToPlayer(GameObject player)
    {
        cam.Follow = player.transform;
    }
}
