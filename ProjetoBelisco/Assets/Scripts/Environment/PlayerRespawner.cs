using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerRespawner : MonoBehaviour
{
    public static PlayerRespawner Instance = null;
    [SerializeField] private float _timeToSpawn = 3f;
    [SerializeField] [EnumToggleButtons] private ScenesIndex.UiScenes _respawnUi = ScenesIndex.UiScenes.RespawnUi;

    public static System.Action<PlayerRespawner> OnStartTimer;

    public float TotalTimeSpam { get; private set; }
    public float RemainigTime { get => _timeToSpawn - TotalTimeSpam ; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PlayerLife.OnPlayerDie += ListenPlayerDeath;
    }

    private void OnDestroy()
    {
        PlayerLife.OnPlayerDie -= ListenPlayerDeath;
    }

    public void StartPlayerRespawnProcess()
    {
        StartCoroutine(TimeToSpawnPlayer());
    }

    private IEnumerator TimeToSpawnPlayer()
    {
        TotalTimeSpam = 0f;
        OnStartTimer?.Invoke(this);

        while (TotalTimeSpam <= _timeToSpawn)
        {
            TotalTimeSpam += Time.deltaTime;
            yield return null;
        }

        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        PlayerLife.RespawnPlayer(this.transform);
    }

    private void ListenPlayerDeath()
    {
        UiScenesLoader.LoadScene(_respawnUi);
    }
}
