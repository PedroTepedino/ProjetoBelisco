﻿using System;
using Cinemachine;
using UnityEngine;

namespace Belisco
{
    public class CameraPriorityTrigger : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _camera.Priority = 10;
                _camera.Follow = other.transform;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _camera.Priority = 1;
                _camera.Follow = null;
            }
        }

        private void OnValidate()
        {
            if (_camera == null)
            {
                _camera = this.GetComponent<CinemachineVirtualCamera>();
            }
            
            Collider2D collider = this.GetComponent<Collider2D>(); 
            if (!collider)
            {
                this.gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
            }
            else
            {
                collider.isTrigger = true;
            }
        }
    }
}