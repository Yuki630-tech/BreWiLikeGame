public class EnemyIdleState : IState<EnemyBase>
{
    private float currentTime = 0f;
    private PatrolEnemyBase patrol;
    public void Enter(EnemyBase owner)
    {
        currentTime = 0f;
        patrol = owner as PatrolEnemyBase;
        owner.NavmeshAgent.isStopped = true;
    }

    public void Update(EnemyBase owner, float deltaTime)
    {
        currentTime += deltaTime;

        if(currentTime >= patrol.IdleTime)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Patrol);
        }
    }

    public void Exit(EnemyBase owner)
    {

    }

}
