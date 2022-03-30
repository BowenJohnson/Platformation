using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Keeps track of the current state and has functions
// to switch between states in the AI state machine
public class FiniteStateMachine
{
    public State currState { get; private set; }

    // set current state to the passed in starting state
    // then run its enter state function
    public void Initialize(State startingState)
    {
        currState = startingState;
        currState.Enter();
    }

    // change from current state over to the next
    // and start its enter state function
    public void ChangeState(State nextState)
    {
        currState.Exit();
        currState = nextState;
        currState.Enter();
    }
}
