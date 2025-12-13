using Cysharp.Threading.Tasks.Triggers;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemyPatrolState : IState<EnemyBase>
{
    private int patrolNum = 0;
    private Vector3 destination;
    private float distance;
    private CompositeDisposable disposables;
    public void Enter(EnemyBase owner)
    {
        PatrolEnemyBase patrol = owner as PatrolEnemyBase;
        disposables = new CompositeDisposable();
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

        ComponentProvider.Instance.Player.PlayerSpeedProperty.Where(x => x >= ComponentProvider.Instance.Player.NoticedByEnemySpeed).Subscribe(_ => owner.IsMoveByTargetSensor = true)
            .AddTo(disposables);
    }

    public async void Update(EnemyBase owner, float deltaTime)
    {
        distance = Vector3.Distance(destination, owner.transform.position);
        if(distance <= 0.05f)
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Idle);
        }

        if(owner.TargetSensor.State == TargetSensor.SensorState.Alert && owner.IsMoveByTargetSensor && ComponentProvider.Instance.CanPlayerBeNoticed())
        {
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Alert);
        }

        if(owner.TargetSensor.State == TargetSensor.SensorState.Chase && owner.IsMoveByTargetSensor && ComponentProvider.Instance.CanPlayerBeNoticed())
        {
            await owner.ShowChaseUITask();
            owner.StateMachine.ChangeState(owner, EnemyBase.EnemyState.Chase);
        }
    }

    public void Exit(EnemyBase owner)
    {
        owner.Animator.SetBool(AnimationParametaName.Move, false);
        disposables.Dispose();
    }

}
