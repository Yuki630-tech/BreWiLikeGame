
public interface IStrategyWithCondition : IState<EnemyBase>
{
    public bool CanStartStrategy(EnemyBase owner);
}
