using UnityEngine;

public class PatrolEnemyBase : EnemyBase
{
    [Header("アイドルステートに関する設定")]
    [Tooltip("どれだけの時間立ち止まるか"), SerializeField] private float idleTime = 1f;
    [Header("パトロールステートに関する設定, 値")]
    [Tooltip("パトロールする範囲となる円の半径"), SerializeField] private float patrolRadius = 2f;
    [Tooltip("この回数パトロールしたら次にパトロールステートに入った時に中央に戻る"), SerializeField] private int patrolNumBackToCenter = 10;

    [Header("パトロールする円の中心"), ReadOnly, SerializeField] private Vector3 patrolCenter;

    public float PatrolRadius { get => patrolRadius; }
    public int PatrolNumBackToCenter { get => patrolNumBackToCenter; }
    public Vector3 PatrolCenter { get => patrolCenter;}
    public float IdleTime { get => idleTime; }

    protected override void Awake()
    {
        base.Awake();

        patrolCenter = transform.position;
        stateMachine.AddState(EnemyState.Idle, new EnemyIdleState());
        stateMachine.AddState(EnemyState.Patrol, new EnemyPatrolState());
        stateMachine.ChangeState(this, EnemyState.Idle);
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        patrolCenter = transform.position;
    }
#endif

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(patrolCenter, patrolRadius);
    }
}
