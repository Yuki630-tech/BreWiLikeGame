using UnityEngine;

public class EnemyAlertState : IState<EnemyBase>
{
    public void Enter(EnemyBase owner)
    {
        owner.NavmeshAgent.isStopped = true;
    }

    public void Update(EnemyBase owner, float deltaTime)
    {
        Vector3 direction = (ComponentProvider.Instance.PlayerTrans.position - owner.transform.position).normalized;
        owner.transform.rotation = Quaternion.LookRotation(direction);

        if(owner.TargetSensor.State == TargetSensor.SensorState.None)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Idle);
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
