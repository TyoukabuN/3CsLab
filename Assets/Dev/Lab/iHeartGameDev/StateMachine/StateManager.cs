using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager<EState> :MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> CurrentState;

    protected bool IsTransitioningState = false;
    public void Init()
    {
        
    }
    public void Update()
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
    public void TransitionToState(EState stateKey)
    {
        if (CurrentState != null) CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
    }
    public void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }
    public void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }
    public void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }
}
