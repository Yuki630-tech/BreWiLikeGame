using UnityEngine;
using UniRx;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

public class EnemyAlertState : IState<EnemyBase>
{
    private float calmness; //敵の穏やか度・・・プレイヤーとの距離が近いほど低くなる
    /// <summary>
    /// 
    /// </summary>
    private bool isChaseStart;　
    private bool canAddCalmness;
    private float reduceInterval = 0.8f;
    private float calmnessAddSpeed = 2f;
    private CancellationTokenSource cts = new();
    private CompositeDisposable disposable;
    public void Enter(EnemyBase owner)
    {
        disposable = new CompositeDisposable();
        isChaseStart = false;
        canAddCalmness = true;
        owner.NavmeshAgent.isStopped = true;

        //プレイヤーが一定の速度で動いていた場合はその距離を?ゲージに適用させる(?ゲージを下げていくフラグをオフにする)
        owner.DistanceProperty.Where(_ => ComponentProvider.Instance.PlayerNoiseSource.IsNoisy.Value).Subscribe( x =>
        {
            calmness = x;
            canAddCalmness = false;
        }).AddTo(disposable);

        //プレイヤーが一定時間止まっていたら?ゲージを下げていくフラグを立てる
        owner.DistanceProperty.Delay(TimeSpan.FromSeconds(reduceInterval)).Subscribe(_ => canAddCalmness = true).AddTo(disposable);
    }

    public async void Update(EnemyBase owner, float deltaTime)
    {
        Vector3 direction = (ComponentProvider.Instance.PlayerTrans.position - owner.transform.position).normalized;
        if (!isChaseStart)
        {
            if (canAddCalmness)
            {
                calmness += calmnessAddSpeed * deltaTime;
            }
            owner.SetNormalizedProximity(calmness);
        }
        owner.transform.rotation = Quaternion.LookRotation(direction);

        if(owner.TargetSensor.State == TargetSensor.SensorState.None)
        {
            owner.IsMoveByTargetSensor = true;
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Idle);
        }

        //プレイヤーが止まったまま?ゲージが0以下になったら
        if(calmness >= owner.TargetSensor.AlertDistance)
        {
            //ターゲットセンサーがアラート状態のままだとAlertステートとIdleステートとの間で無限ループになってしまうので
            //いったんTargetSensorによるステート操作をオフにした状態でパトロールモードに
            owner.IsMoveByTargetSensor = false; 
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Idle);
        }

        //プレイヤーが一定以上の速度で追跡範囲内に入ってきたら
        if(owner.TargetSensor.State == TargetSensor.SensorState.Chase && !isChaseStart && ComponentProvider.Instance.PlayerNoiseSource.IsNoisy.Value)
        {
            isChaseStart = true;
            owner.SetNormalizedProximityToEndValue();
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
