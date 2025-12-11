using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackStrategyFactory : IRandomStrategyFactory
{
    private List<IStrategyWithCondition> attackStrategyList = new();
    private IStrategyWithCondition currentStrategy;
    public void AddStrategy(IStrategyWithCondition value)
    {
        attackStrategyList.Add(value);
    }

    public void CreateStrategy()
    {
        currentStrategy = attackStrategyList[Random.Range(0, attackStrategyList.Count)];
    }

    public IStrategyWithCondition GetStrategy()
    {
        return currentStrategy;
    }
}
