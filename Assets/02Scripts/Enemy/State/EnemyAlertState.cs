using UnityEngine;

public class EnemyAlertState : IState<EnemyBase>
{
    private float distance;
    private bool isChaseStart;
    public void Enter(EnemyBase owner)
    {
        isChaseStart = false;
        owner.NavmeshAgent.isStopped = true;
        Debug.Log("ÉAÉâÅ[Ég!");
    }

    public async void Update(EnemyBase owner, float deltaTime)
    {
        distance = Vector3.Distance(owner.transform.position, ComponentProvider.Instance.PlayerTrans.position);
        Vector3 direction = (ComponentProvider.Instance.PlayerTrans.position - owner.transform.position).normalized;
        if (!isChaseStart)
        {
            owner.SetNormalizedProximity(distance);
        }
        owner.transform.rotation = Quaternion.LookRotation(direction);

        if(owner.TargetSensor.State == TargetSensor.SensorState.None)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Idle);
        }

        if(owner.TargetSensor.State == TargetSensor.SensorState.Chase && !isChaseStart)
        {
            isChaseStart = true;
            
            await owner.ShowChaseUITask();
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Chase);
            return;
        }


    }

    public void Exit(EnemyBase owner)
    {
        owner.SetNormalizedProximityToEndValue();
    }

}
