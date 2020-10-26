using System;
using System.Collections;
using UnityEngine;

namespace Belisco
{
    public class PlayerRespawner : MonoBehaviour
    {
        public static PlayerRespawner CurrentSpawner;

        public static Action<PlayerRespawner> OnStartTimer;
        [SerializeField] private float _timeToSpawn = 3f;
        [SerializeField] private bool _firstSpawner;

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
                CurrentSpawner = this;
            else if (_firstSpawner) CurrentSpawner = this;

            Player.OnPlayerDeath += ListenPlayerDeath;
        }

        private void Start()
        {
            if (Pooler.Instance.CountSpawnedInstances("Player") < 1 && _firstSpawner) RespawnPlayer();
        }

        private void OnEnable()
        {
            if (_firstSpawner) CurrentSpawner = this;
        }

        private void OnDestroy()
        {
            Player.OnPlayerDeath -= ListenPlayerDeath;
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
            Pooler.Instance.MoveObjectToPoint("Player", transform);
            StartCoroutine(Teste());
        }

        private IEnumerator Teste()
        {
            yield return new WaitForEndOfFrame();
            Pooler.Instance.SpawnFromPool("Player", transform);
        }


        private void ListenPlayerDeath()
        {
            if (gameObject == CurrentSpawner.gameObject)
                RespawnPlayer();
            //UiScenesLoader.LoadScene(_respawnUi);
        }

        public void SetToCurrentRespawner()
        {
            CurrentSpawner = this;
        }
    }
}