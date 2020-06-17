using System;
using System.Collections;
using System.Collections.Generic;
using RefatoramentoDoTioTepe;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    private void Awake()
    {
        _panel.SetActive(false);
        GameStateMachine.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateMachine.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(IState state)
    {
        _panel.SetActive(state is Pause);
    }
}
