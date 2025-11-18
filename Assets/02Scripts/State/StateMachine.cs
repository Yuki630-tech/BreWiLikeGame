using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TTypeId, T> where TTypeId : Enum where T : MonoBehaviour
{
    Dictionary<TTypeId, IState<T>> states = new Dictionary<TTypeId, IState<T>>();
    IState<T> currentState;

    public TTypeId TypeId {  get; private set; }

    public void AddState(TTypeId setId, IState<T> setState)
    {
        states[setId] = setState;
    }

    public void ChangeState(T owner, TTypeId setId)
    {
        if(currentState != null)
        {
            currentState.Exit(owner);
        }
        currentState = states[setId];
        TypeId = setId;
        currentState.Enter(owner);
    }

    public void Update(float deltaTime, T owner)
    {
        currentState.Update(owner, deltaTime);
    }
}
