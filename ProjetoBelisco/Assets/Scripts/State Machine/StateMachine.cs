using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/*Class: StateMachine
 * Class that describes the behavior of a state machine and handle states
*/
public class StateMachine : MonoBehaviour
{
    // Group: Private Variables

    /* Variables: States
     * currentState - Hold the current in execution state
     * previousState - Hold the previous executed state
     */
    private IState currentState { get; set; }
    private IState previousState;

    // Group: State Management Logic

    /* Function: ChangeState
     * Change the current state to a new state and save the previous state.
     * Parameters:
     * newState - A state that implents <IState>, which will be the new current state.
     */
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

    /* Function: SwitchToPreviousState
     * Switch back to the state executed before the current state.
     */
    public void SwitchToPreviousState()
    {
        if (previousState != null)
        {
            ChangeState(previousState);
        }
    }

    // Group: State Execution Logic

    /* Function: RunStateUpdate
     * Execute Update Time commands for the current state. 
     * 
     * *Must be in the Update function.*
     */
    public void RunStateUpdate()
    {
        if (currentState != null)
        {
            this.currentState.RunState();
        }
    }
}