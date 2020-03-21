using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ColletableTypes
{
    Paw = 1
}

public class CollectableSpawner : SerializedMonoBehaviour
{
    [SerializeField] private bool _repickable = false;
    [SerializeField] [EnumToggleButtons] private ColletableTypes _colletableType;

    private GameObject _collectable = null;

    public bool HasBeenPicked { get; private set; } = false;

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
            _collectable = ObjectPooler.Instance.SpawnFromPool(_colletableType.ToString(), this.transform);
            _collectable.GetComponent<BaseCollectableObject>().OnPickUp += ListenPicked;
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

        Gizmos.DrawLine(this.transform.position, Camera.main.transform.position);
    }
}
