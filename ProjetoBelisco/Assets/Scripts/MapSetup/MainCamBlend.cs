using System;
using Cinemachine;
using UnityEngine;

namespace Belisco
{
    public class MainCamBlend : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain _cam;
        [SerializeField] private CompositeCollider2D _collider;

        private void Update()
        {
            _collider.gameObject.SetActive(_cam.IsBlending);
        }

        private void OnValidate()
        {
            if (_cam == null)
            {
                _cam = this.GetComponent<CinemachineBrain>();
            }
            
            if (_collider == null)
            {
                _collider = this.GetComponentInChildren<CompositeCollider2D>();
            }
        }
    }
}