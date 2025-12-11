using UnityEngine;

public class EnemyChaseState : IState<EnemyBase>
{
    
    public async void Enter(EnemyBase owner)
    {
        
        owner.NavmeshAgent.isStopped = false;
        owner.Animator.SetBool(AnimationParametaName.Run, true);
        owner.NavmeshAgent.speed = owner.ChaseSpeed;
    }

    public void Update(EnemyBase owner, float deltaTime)
    {
        owner.NavmeshAgent.SetDestination(ComponentProvider.Instance.PlayerTrans.position);
        float distance = Vector3.Distance(owner.transform.position, owner.DefaultPosition);
        if(owner.TargetSensor.State == TargetSensor.SensorState.None || distance >= owner.DistanceBackToDefaultPosition)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Back);
        }

        if(owner.TargetSensor.State == TargetSensor.SensorState.Strafe)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Strafe);
        }
    }

    public void Exit(EnemyBase owner)
    {
        owner.Animator.SetBool(AnimationParametaName.Run, false);
    }

}
