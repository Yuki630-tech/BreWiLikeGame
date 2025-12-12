using UnityEngine;

public class GoblinStrafeState : IState<EnemyBase>
{
    SwordGoblinEnemy swordGogline;
    public void Enter(EnemyBase owner)
    {
        owner.NavmeshAgent.updateRotation = false;
        owner.NavmeshAgent.isStopped = false;
        owner.Animator.SetBool(AnimationParametaName.Move, true);
        owner.EnemyAttackStrategyFactory.CreateStrategy();
        swordGogline = owner as SwordGoblinEnemy;
        owner.NavmeshAgent.speed = swordGogline.StrafeSpeed;

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
            Debug.Log("çUåÇâ¬î\");
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Attack);
        }
    }

    public void Exit(EnemyBase owner)
    {
        owner.Animator.SetBool(AnimationParametaName.Move, false);
        owner.NavmeshAgent.updateRotation = true;
        owner.NavmeshAgent.isStopped = true;

    }

}
