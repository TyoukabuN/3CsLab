using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> CurrentState;

    protected bool IsTransitioningState = false;
    private void Start()
    {
        
    }
    private void Update()
    {
        var nextStateKey = CurrentState.GetNextState();

        if (CurrentState.StateKey.Equals(nextStateKey))
        {
            CurrentState.UpdateState();
        }
        else
        {
            TransitionToState(nextStateKey);
        }
    }
    private void TransitionToState(EState stateKey)
    {
        if (CurrentState != null) CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
    }
    private void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }
    private void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }
    private void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }
}
