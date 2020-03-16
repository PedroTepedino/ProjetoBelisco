using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] private float _timeToSpwan = 3f;

    private void Awake()
    {
        PlayerLife.OnPlayerDie += StartPlayerRespawnProcess;
    }

    private void OnDestroy()
    {
        PlayerLife.OnPlayerDie -= StartPlayerRespawnProcess;
    }

    private void StartPlayerRespawnProcess()
    {
        StartCoroutine(TimeToSpawnPlayer());
    }

    private IEnumerator TimeToSpawnPlayer()
    {
        float totalTime = 0f;

        while (totalTime <= _timeToSpwan)
        {
            totalTime += Time.deltaTime;
            Debug.Log(totalTime);
            yield return null;
        }

        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        PlayerLife.RespawnPlayer(this.transform);
    }
}
