using UnityEngine;
using Rewired;

namespace Player.NewPlayer
{
    public class RewiredPlayerInput : IPlayerInput
    {
        private Rewired.Player _rewiredPlayer;

        public float Horizontal => _rewiredPlayer.GetAxis("MoveHorizontal");

        public RewiredPlayerInput(Rewired.Player playerRewiredController)
        {
            _rewiredPlayer = playerRewiredController;
        }
    }
}