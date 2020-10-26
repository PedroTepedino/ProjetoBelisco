using UnityEngine;

namespace Belisco
{
    public abstract class AbstractPanel : MonoBehaviour
    {
        [SerializeField] protected GameObject _panel;

        protected virtual void Awake()
        {
            _panel.SetActive(false);
            GameStateMachine.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDestroy()
        {
            GameStateMachine.OnGameStateChanged -= HandleGameStateChanged;
        }

        protected abstract void HandleGameStateChanged(IState state);
    }
}