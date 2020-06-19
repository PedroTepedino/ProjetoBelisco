using System;
using System.Collections;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class RewiredPlayerInput : MonoBehaviour, IPlayerInput
    {
        private Rewired.Player _rewiredPlayer;

        public static IPlayerInput Instance { get; set; } 
        
        public float Horizontal => _rewiredPlayer.GetAxis("MoveHorizontal");

        public bool PausePressed => _rewiredPlayer.GetButtonDown("Pause");

        private void Awake()
        {
            Instance = this;
            StartCoroutine(GetPlayer());
        }
        
        private IEnumerator GetPlayer()
        {
            while (_rewiredPlayer == null)
            {
                yield return null;
                if (Rewired.ReInput.isReady)
                    _rewiredPlayer = Rewired.ReInput.players.GetPlayer(0);
            }
        }
    }
}