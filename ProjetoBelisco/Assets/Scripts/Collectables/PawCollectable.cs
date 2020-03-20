using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawCollectable : MonoBehaviour, ICollectableObject, IPooledObject
{
    [SerializeField] private int _pawsToAdd = 1;

    public void PickUp()
    {
        Destroy(this.gameObject);
    }

    public void Effect()
    {
        PlayerPawStorage.AddPaws(_pawsToAdd);
    }

    public void OnObjectSpawn()
    {
        this.gameObject.SetActive(true);
    }
}
