using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerRespawner : MonoBehaviour
{
    public static PlayerRespawner CurrentSpawner = null;
    [SerializeField] private float _timeToSpawn = 3f;
    [SerializeField] [EnumToggleButtons] private ScenesIndex.UiScenes _respawnUi = ScenesIndex.UiScenes.RespawnUi;
    [SerializeField] bool _firstSpawner = false;

    public static System.Action<PlayerRespawner> OnStartTimer;

    public float TotalTimeSpam { get; private set; }
    public float RemainigTime => _timeToSpawn - TotalTimeSpam;

#if UNITY_EDITOR
    public bool IsFirstSpawner
    {
        get => _firstSpawner;
        set => _firstSpawner = value;
    }
#endif

    private void Awake()
    {
        if (CurrentSpawner == null)
        {
            CurrentSpawner = this;
        }
        else if (_firstSpawner)
        {
            CurrentSpawner = this;
        }

        PlayerLife.OnPlayerDie += ListenPlayerDeath;
    }

    private void OnDestroy()
    {
        PlayerLife.OnPlayerDie -= ListenPlayerDeath;
    }

    private void Start()
    {
        if (ObjectPooler.Instance.CountSpawnedInstances("Player") < 1 && _firstSpawner)
        {
            RespawnPlayer();
        }
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
        ObjectPooler.Instance.MoveObjectToPoint("Player", this.transform);
        StartCoroutine(Teste());
    }

    private IEnumerator Teste()
    {
        yield return new WaitForEndOfFrame();
        ObjectPooler.Instance.SpawnFromPool("Player", this.transform);
    }
    

    private void ListenPlayerDeath()
    {
        if (this.gameObject == CurrentSpawner.gameObject)
        {
            RespawnPlayer();
            //UiScenesLoader.LoadScene(_respawnUi);
        }
    }

    public void SetToCurrentRespawner()
    {
        CurrentSpawner = this;
    }
}
