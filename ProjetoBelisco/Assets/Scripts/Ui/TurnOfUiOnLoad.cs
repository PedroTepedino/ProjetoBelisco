using System.Collections;
using UnityEngine;

namespace Belisco
{
    public class TurnOfUiOnLoad : MonoBehaviour
    {
        [SerializeField] private GameObject _uiCanvas;

        private void OnEnable()
        {
            GameStateMachine.OnGameStateChanged += ListenOnChangedGameState;
        }

        private void OnDisable()
        {
            GameStateMachine.OnGameStateChanged -= ListenOnChangedGameState;
        }

        private void ListenOnChangedGameState(IState state)
        {
            if (state is LoadLevel)
                _uiCanvas.SetActive(false);
            else
                StartCoroutine(WaitForFadeIn());
        }

        private IEnumerator WaitForFadeIn()
        {
            yield return new WaitWhile(() => FadeInOutSceneTransition.Instance.FadeInCompleted);

            _uiCanvas.SetActive(true);
        }
    }
}