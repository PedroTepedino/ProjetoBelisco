using System;
using System.Collections;
using GameScripts.PoolingSystem;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class PlayerGhostSpawner : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _rightSpriteRenderer;
        [SerializeField] private SpriteRenderer _leftSpriteRenderer;

        [SerializeField] private Player _player;

        [SerializeField] private float _timeBetweenSpawns = 0.05f;
        [SerializeField] private int _ghostsToSpawn = 3;

        private WaitForSeconds _waitForSeconds;

        private void Awake()
        {
            _waitForSeconds = new WaitForSeconds(_timeBetweenSpawns);
        }

        public void StartSpawning()
        {
            StartCoroutine(GhostSpawning());
        }

        private IEnumerator GhostSpawning()
        {
            for (int i = 0; i < _ghostsToSpawn; i++)
            {
                yield return _waitForSeconds;
                SpawnGhost();
            }   
        }

        private void SpawnGhost()
        {
            SpriteRenderer currentRenderer = _rightSpriteRenderer.enabled
                ? _rightSpriteRenderer
                : _leftSpriteRenderer;
            Pooler.Instance.SpawnFromPool("Ghost",currentRenderer.transform.position, Quaternion.identity, new object[] {currentRenderer.sprite});
        }

        private void OnValidate()
        {
            if (_player == null)
            {
                _player = this.GetComponent<Player>();
            }
        }
    }
}