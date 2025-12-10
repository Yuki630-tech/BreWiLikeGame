using UnityEngine;
public class EnemyBackState : IState<EnemyBase>
{
    public void Enter(EnemyBase owner)
    {
        owner.NavmeshAgent.SetDestination(owner.DefaultPosition);
    }

    public void Update(EnemyBase owner, float deltaTime)
    {
        float distance = Vector3.Distance(owner.transform.position, owner.DefaultPosition);

        if(distance <= 0.1f)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Idle);
        }
    }

    public void Exit(EnemyBase owner)
    {

    }

}
