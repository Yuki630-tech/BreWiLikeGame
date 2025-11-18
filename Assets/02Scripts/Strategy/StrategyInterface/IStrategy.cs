using System;
using UnityEngine;

public interface IStrategy<T> where T : MonoBehaviour
{
    public void Enter(T owner);
    public void Update(T owner, float deltaTime);
    public void Exit(T owner);
}
