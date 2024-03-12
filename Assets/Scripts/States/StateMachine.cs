using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState previousState;
    private IState currentState;
    private bool changingStates;

    public void ExecuteStateUpdate()
    {
        if (currentState != null && !changingStates)
        {
            currentState.Execute();
        }
    }

    public void ChangeState(IState newState)
    {
        changingStates = true;
        if (currentState != null)
        {
            currentState.Exit();
        }
        previousState = currentState;
        currentState = newState;
        currentState.Enter();
        changingStates = false;
    }
}
