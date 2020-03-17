using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StateMachine : MonoBehaviour
{
    private IState currentState { get; set; }
    private IState previousState;
    public void ChangeState(IState newState)
    {
        if (this.currentState != null)
        {
            this.currentState.ExitState();
        }

        this.previousState = currentState;
        this.currentState = newState;
        this.previousState.EnterState();
        this.currentState.EnterState();
    }
    public void RunStateUpdate()
    {
        if (currentState != null)
        {
            this.currentState.RunState();
        }
    }
    public void SwitchToPreviousState()
    {
        if (previousState != null)
        {
            ChangeState(previousState);
        }
    }
}