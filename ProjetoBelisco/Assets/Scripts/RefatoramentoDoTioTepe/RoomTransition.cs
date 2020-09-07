using System;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Collider2D))]
    public class RoomTransition : MonoBehaviour
    {
        [SerializeField] private string _sceneToTransition;
        [SerializeField] private int _transitionId;

        private void OnCollisionEnter2D(Collision2D other)
        {
            var player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                LoadLevel.LevelToLoad = _sceneToTransition;
                player.gameObject.SetActive(false);
            }
        }
    }
}