using UnityEngine;

public class PatrolEnemyBase : EnemyBase
{
    
    protected override void Awake()
    {
        base.Awake();

        stateMachine.AddState(EnemyState.Patrol, new EnemyPatrolState());
    }
}
