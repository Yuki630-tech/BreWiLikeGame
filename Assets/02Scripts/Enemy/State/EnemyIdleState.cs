using UnityEngine;

public class EnemyIdleState : IState<EnemyBase>
{
    private float currentTime = 0f;
    private PatrolEnemyBase patrol;
    public void Enter(EnemyBase owner)
    {
        currentTime = 0f;
        patrol = owner as PatrolEnemyBase;
        owner.NavmeshAgent.isStopped = true;
        owner.Animator.SetBool(AnimationParametaName.Move, false);
    }

    public void Update(EnemyBase owner, float deltaTime)
    {
        currentTime += deltaTime;
        Debug.Log(owner.gameObject.name + "Idle’†‚ÌcurrentTime : " + currentTime);

        if(currentTime >= patrol.IdleTime && owner.TargetSensor.State == TargetSensor.SensorState.None)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Patrol);
        }

        if(owner.TargetSensor.State == TargetSensor.SensorState.Alert)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Alert);
        }

        if(owner.TargetSensor.State == TargetSensor.SensorState.Chase)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Chase);
        }
    }

    public void Exit(EnemyBase owner)
    {

    }

}
