using GameScripts.PoolingSystem;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class GhostSpawner : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;

        [SerializeField] private float _timeBetweenSpawns = 0.5f;
        private float _timer = 0f;

        private void Update()
        {
            if (_timer <= 0f)
            {
                SpawnGhost();
                _timer = _timeBetweenSpawns;
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }

        private void SpawnGhost()
        {
            Pooler.Instance.SpawnFromPool("Ghost",this.transform.position, Quaternion.identity, new object[] {_renderer.sprite});
        }

        private void OnValidate()
        {
            if (_renderer == null)
            {
                _renderer = this.GetComponent<SpriteRenderer>();
            }
        }
    }
}