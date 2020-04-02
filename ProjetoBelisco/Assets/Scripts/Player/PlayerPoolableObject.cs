using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolableObject : MonoBehaviour, IPooledObject
{
    public void OnObjectSpawn()
    {
        this.GetComponent<PlayerLife>().RespawnPlayer();
    }
}
