using Unity.VisualScripting;

public class EnemyAttackState : IState<EnemyBase>
{

    public void Enter(EnemyBase owner)
    {
        owner.EnemyAttackStrategyFactory.GetStrategy().Enter(owner);
    }

    public void Update(EnemyBase owner, float deltaTime)
    {
        owner.EnemyAttackStrategyFactory.GetStrategy().Update(owner, deltaTime);
    }

    public void Exit(EnemyBase owner)
    {
        owner.EnemyAttackStrategyFactory.GetStrategy().Exit(owner);
    }

}
