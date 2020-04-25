using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeToSpecialCam : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCam = null;

    private void Awake()
    {
        if (_virtualCam == null)
        {
            _virtualCam = this.GetComponentInChildren<CinemachineVirtualCamera>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _virtualCam.Priority = 1;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _virtualCam.Priority = -1;
    }
}
