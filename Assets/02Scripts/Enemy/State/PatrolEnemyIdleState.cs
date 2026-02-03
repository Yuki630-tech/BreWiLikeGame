using UniRx;
using UnityEngine;

public class PatrolEnemyIdleState : IState<EnemyBase>
{
    private float currentTime = 0f;
    private PatrolEnemyBase patrol;
    private CompositeDisposable disposables;
    public void Enter(EnemyBase owner)
    {
        disposables = new();
        currentTime = 0f;
        patrol = owner as PatrolEnemyBase;
        owner.NavmeshAgent.isStopped = true;
        owner.Animator.SetBool(AnimationParametaName.Move, false);
        ComponentProvider.Instance.PlayerNoiseSource.IsNoisy.Where(x => x).Subscribe(_ => owner.IsMoveByTargetSensor = true).AddTo(disposables);
    }

    public async void Update(EnemyBase owner, float deltaTime)
    {

        currentTime += deltaTime;
        //Debug.Log(owner.gameObject.name + "Idle’†‚ÌcurrentTime : " + currentTime);

        if(currentTime >= patrol.IdleTime)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Patrol);
        }

        if(owner.TargetSensor.State == TargetSensor.SensorState.Alert && owner.IsMoveByTargetSensor && ComponentProvider.Instance.PlayerNoiseSource.IsNoisy.Value)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Alert);
        }

        if(owner.TargetSensor.State == TargetSensor.SensorState.Chase && owner.IsMoveByTargetSensor && ComponentProvider.Instance.PlayerNoiseSource.IsNoisy.Value)
        {
            await owner.ShowChaseUITask();
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Chase);
        }

        if (owner.TargetSensor.State == TargetSensor.SensorState.Strafe && owner.IsMoveByTargetSensor && ComponentProvider.Instance.PlayerNoiseSource.IsNoisy.Value)
        {
            await owner.ShowChaseUITask();
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Strafe);
        }
    }

    public void Exit(EnemyBase owner)
    {
        disposables.Dispose();
    }

}
