using UnityEngine;
using UniRx;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class EnemyAlertState : IState<EnemyBase>
{
    private float distance;
    private bool isChaseStart;
    private bool canReduceDistance;
    private float reduceInterval = 0.8f;
    private float reduceSpeed = 2f;
    private CancellationTokenSource cts = new();
    private CompositeDisposable disposable;
    public void Enter(EnemyBase owner)
    {
        disposable = new CompositeDisposable();
        isChaseStart = false;
        canReduceDistance = true;
        owner.NavmeshAgent.isStopped = true;
        owner.DistanceProperty.Where(_ => ComponentProvider.Instance.Player.PlayerSpeedProperty.Value >= ComponentProvider.Instance.Player.NoticedByEnemySpeed).Subscribe( x =>
        {
            distance = x;
            canReduceDistance = false;
        }).AddTo(disposable);

        owner.DistanceProperty.Delay(TimeSpan.FromSeconds(reduceInterval)).Subscribe(_ => canReduceDistance = true).AddTo(disposable);
    }

    public async void Update(EnemyBase owner, float deltaTime)
    {
        Vector3 direction = (ComponentProvider.Instance.PlayerTrans.position - owner.transform.position).normalized;
        if (!isChaseStart)
        {
            if (canReduceDistance)
            {
                distance += reduceSpeed * deltaTime;
            }
            owner.SetNormalizedProximity(distance);
        }
        owner.transform.rotation = Quaternion.LookRotation(direction);

        if(owner.TargetSensor.State == TargetSensor.SensorState.None)
        {
            owner.IsMoveByTargetSensor = true;
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Idle);
        }

        if(distance >= owner.TargetSensor.AlertDistance)
        {
            owner.IsMoveByTargetSensor = false;
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Idle);
        }

        if(owner.TargetSensor.State == TargetSensor.SensorState.Chase && !isChaseStart)
        {
            isChaseStart = true;
            await owner.ShowChaseUITask();
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Chase);
        }


    }

    public void Exit(EnemyBase owner)
    {
        owner.SetNormalizedProximityToEndValue();
        Debug.Log("アラート終了");
        disposable.Dispose();
    }

}
