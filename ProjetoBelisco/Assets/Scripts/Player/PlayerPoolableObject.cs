using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolableObject : MonoBehaviour, IPooledObject
{
    private PlayerLife _playerLife;

    private void Awake()
    {
        _playerLife = this.GetComponent<PlayerLife>();
    }

    public void OnObjectSpawn()
    {
        _playerLife.RespawnPlayer();
    }
}
