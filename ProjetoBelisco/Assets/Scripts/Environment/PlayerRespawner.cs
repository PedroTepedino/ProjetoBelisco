using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerRespawner : MonoBehaviour
{
    public static PlayerRespawner Instance = null;
    [SerializeField] private float _timeToSpawn = 3f;
    [SerializeField] [EnumToggleButtons] private ScenesIndex.UiScenes _respawnUi = ScenesIndex.UiScenes.RespawnUi;
    [SerializeField] bool _firstSpawner = false;

    public static System.Action<PlayerRespawner> OnStartTimer;

    public float TotalTimeSpam { get; private set; }
    public float RemainigTime { get => _timeToSpawn - TotalTimeSpam ; }

    public bool IsFirstSpawner { get => _firstSpawner; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PlayerLife.OnPlayerDie += ListenPlayerDeath;
    }

    private void Start()
    {
        if (ObjectPooler.Instance.CountSpawnedInstances("Player") < 1 && _firstSpawner)
        {
            RespawnPlayer();
        }
    }

    private void OnDestroy()
    {
        PlayerLife.OnPlayerDie -= ListenPlayerDeath;
    }

    public void StartPlayerRespawnProcess()
    {
        StartCoroutine(routine: TimeToSpawnPlayer());
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
        ObjectPooler.Instance.SpawnFromPool("Player", this.transform, parent: false);
    }

    private void ListenPlayerDeath()
    {
        UiScenesLoader.LoadScene(_respawnUi);
    }
}
