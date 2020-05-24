﻿using GameScripts.Camera;
using GameScripts.PoolingSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameScripts.Collectables
{
    public enum CollectableTypes
    {
        Paw = 1
    }

    public class Spawner : MonoBehaviour
    {
        [SerializeField] private bool _repickable = false;
        [SerializeField] [EnumToggleButtons] private CollectableTypes _colletableType;

        private GameObject _collectable = null;

        public bool HasBeenPicked { get; private set; } = false;
        public CollectableTypes CollectableType { get => _colletableType; set => _colletableType = value; }

        private void Update()
        {
            if (CameraObjectVisibilityField.IsWithinBounds(this.transform.position))
            {
                SpawnCollectable();
            }
            else
            {
                DespawnCollectable();
            }
        }

        [Button]
        private void SpawnCollectable()
        {
            if ((_repickable || !HasBeenPicked) && (_collectable == null))
            {
                HasBeenPicked = false;
                _collectable = Pooler.Instance.SpawnFromPool(_colletableType.ToString(), this.transform);
                _collectable.GetComponent<BaseObject>().OnPickUp += ListenPicked;
            }
        }

        [Button]
        private void DespawnCollectable()
        {
            if ((_collectable != null))
            {
                _collectable.SetActive(false);
                _collectable = null;
            }
        }

        private void ListenPicked()
        {
            HasBeenPicked = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            if (CameraObjectVisibilityField.IsWithinBounds(this.transform.position))
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawLine(this.transform.position, UnityEngine.Camera.main.transform.position);
        }
    }
}