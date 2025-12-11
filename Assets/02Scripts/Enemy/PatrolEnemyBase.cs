using UniRx;
using UnityEngine;

public class PatrolEnemyBase : EnemyBase
{
    [Header("アイドルステートに関する設定")]
    [Tooltip("どれだけの時間立ち止まるか"), SerializeField] private float idleTime = 1f;
    [Header("パトロールステートに関する設定, 値")]
    [Tooltip("パトロール中の移動スピード"), SerializeField] private float patrolSpeed = 3.3f;
    [Tooltip("パトロールする範囲となる円の半径(黄)"), SerializeField] private float patrolRadius = 2f;
    [Tooltip("この回数パトロールしたら次にパトロールステートに入った時に中央に戻る"), SerializeField] private int patrolNumBackToCenter = 10;

    [Header("パトロールする円の中心"), ReadOnly, SerializeField] private Vector3 patrolCenter;
   

    public float PatrolRadius { get => patrolRadius; }
    public int PatrolNumBackToCenter { get => patrolNumBackToCenter; }
    public Vector3 PatrolCenter { get => patrolCenter;}
    public float IdleTime { get => idleTime; }
    public float PatrolSpeed { get => patrolSpeed;}

    protected override void Awake()
    {
        base.Awake();

        patrolCenter = transform.position;
        defaultPosition = patrolCenter;
        stateMachine.AddState(EnemyState.Idle, new PatrolEnemyIdleState());
        stateMachine.AddState(EnemyState.Patrol, new PatrolEnemyPatrolState());
        stateMachine.ChangeState(this, EnemyState.Idle);
    }

    private void OnDrawGizmos()
    {
        Vector3 center;
        if (!Application.isPlaying)
        {
            center = transform.position;
        }

        else
        {
            center = patrolCenter;
        }
            Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, patrolRadius);
    }
}
