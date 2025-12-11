using UnityEngine;

public class GoblinStrafeState : IState<EnemyBase>
{
    SwordGoblinEnemy swordGogline;
    public void Enter(EnemyBase owner)
    {
        owner.NavmeshAgent.updateRotation = false;
        owner.Animator.SetBool(AnimationParametaName.Strafe, true);
        owner.EnemyAttackStrategyFactory.CreateStrategy();
        swordGogline = owner as SwordGoblinEnemy;
    }

    public void Update(EnemyBase owner, float deltaTime)
    {
        var direction = (owner.transform.position - ComponentProvider.Instance.PlayerTrans.position).normalized;
        owner.transform.rotation = Quaternion.LookRotation(-direction);
        owner.NavmeshAgent.SetDestination(ComponentProvider.Instance.PlayerTrans.position + ComponentProvider.Instance.PlayerTrans.right * swordGogline.RightOffsetFromPlayerToStrafeDes);

        if(owner.TargetSensor.State == TargetSensor.SensorState.Chase)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Chase);
        }

        if (owner.EnemyAttackStrategyFactory.GetStrategy().CanStartStrategy(owner))
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Attack);
        }
    }

    public void Exit(EnemyBase owner)
    {
        owner.Animator.SetBool(AnimationParametaName.Strafe, false);
    }

}
