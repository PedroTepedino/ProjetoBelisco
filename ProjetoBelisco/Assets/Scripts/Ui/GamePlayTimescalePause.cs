using UnityEngine;

namespace Belisco
{
    public class GamePlayTimescalePause : MonoBehaviour
    {
        private void OnEnable()
        {
            GameStateMachine.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            GameStateMachine.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void HandleGameStateChanged(IState state)
        {
            if (state is Options) Time.timeScale = 0f;
        }
    }
}