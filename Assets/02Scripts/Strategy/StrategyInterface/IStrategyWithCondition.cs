
public interface IStrategyWithCondition : IState<EnemyBase>
{
    public bool CanStartStrategy(EnemyBase owner);

    public string GetName();
}
