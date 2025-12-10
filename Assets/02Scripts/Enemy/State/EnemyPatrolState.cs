using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : IState<EnemyBase>
{
    private int patrolNum = 0;
    private Vector3 destination;
    private float distance;
    public void Enter(EnemyBase owner)
    {
        PatrolEnemyBase patrol = owner as PatrolEnemyBase;

        owner.Animator.SetBool(AnimationParametaName.Move, true);
        owner.NavmeshAgent.isStopped = false;
        owner.NavmeshAgent.speed = patrol.PatrolSpeed;
       
        if (NavMeshUtility.TryGetCirclePosOnNavMesh(patrol.PatrolCenter, patrol.PatrolRadius, out destination) && patrolNum <= patrol.PatrolNumBackToCenter)
        {
            owner.NavmeshAgent.SetDestination(destination);
            patrolNum++;
        }

        else if(patrolNum > patrol.PatrolNumBackToCenter && NavMesh.SamplePosition(patrol.PatrolCenter, out NavMeshHit hit, 0.5f, NavMesh.AllAreas))
        {
            destination = hit.position;
            owner.NavmeshAgent.SetDestination(destination);
            patrolNum = 0;
        }
    }

    public void Update(EnemyBase owner, float deltaTime)
    {
        distance = Vector3.Distance(destination, owner.transform.position);
        if(distance <= 0.05f)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Idle);
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
        owner.Animator.SetBool(AnimationParametaName.Move, false);
    }

}
