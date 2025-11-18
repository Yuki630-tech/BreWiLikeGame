using System;
using UnityEngine;

public interface IStrategyFactory<TType, T> where T : MonoBehaviour where TType : Enum
{
    public IStrategy<T> GetStrategy();
    public void CreateStrategy(T owner, TType type);
    //public void ChangeStrategy(T owner, TType type);
}
