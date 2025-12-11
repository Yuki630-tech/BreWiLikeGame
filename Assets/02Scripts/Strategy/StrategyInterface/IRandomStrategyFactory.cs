using UnityEngine;

public interface IRandomStrategyFactory
{
    public void CreateStrategy();

    public IStrategyWithCondition GetStrategy();

    public void AddStrategy(IStrategyWithCondition value);
}
