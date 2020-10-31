using System;
using UnityEngine;

namespace Belisco
{
    public class RoomSpawner : MonoBehaviour
    {
        public static RoomSpawner CreateSpawner() => new GameObject("[RoomSpawner]").AddComponent<RoomSpawner>();

        
        private static RoomSpawner _currentSpawner = null;
        public static RoomSpawner CurrentSpawner
        {
            get
            {
                if (_currentSpawner == null)
                {
                    var spawner = CreateSpawner();
                    _currentSpawner = spawner;

                    var roomManager = FindObjectOfType<RoomManager>();
                    if (roomManager != null)
                    {
                        _currentSpawner.transform.position = roomManager.transform.position;
                    }
                    else
                    {
                        _currentSpawner.transform.position = Vector3.zero;
                    }
                }

                return _currentSpawner;
            }
        } 

        [SerializeField] private int _priority = 0;

        public int Priority => _priority;
        
        private void Awake()
        {
            if (_currentSpawner == null)
            {
                _currentSpawner = this;
            }
            
            if (this._priority > _currentSpawner._priority)
            {
                this.SetCurrentSpawner();
            }
        }

        public void SetCurrentSpawner()
        {
            _currentSpawner = this;
        }

        private void OnValidate()
        {
            this.name = $"[SPAWNER_{this.gameObject.scene.name}]";
        }
    }
}