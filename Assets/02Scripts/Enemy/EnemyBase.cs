using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(TargetSensor))]
public class EnemyBase : MonoBehaviour
{
    protected StateMachine<EnemyState, EnemyBase> stateMachine = new();

    [Header("コンポーネント群")]
    [Tooltip("NavmeshAgent"), SerializeField] private NavMeshAgent navmeshAgent;
    [Tooltip("Animator"), SerializeField] private Animator animator;
    [Tooltip("TargetSensor"), SerializeField] private TargetSensor targetSensor;

    [Header("Chaseステートに関する設定")]
    [Tooltip("Chase中の移動スピード"), SerializeField] private float chaseSpeed = 5f;

    [Header("拠点に戻る距離"), SerializeField] private float distanceBackToDefaultPosition = 10f;

    [Header("敵のスタート地点"), ReadOnly, SerializeField] protected Vector3 defaultPosition;

    public NavMeshAgent NavmeshAgent { get => navmeshAgent; }
    public Animator Animator { get => animator; }
    public StateMachine<EnemyState, EnemyBase> StateMachine { get => stateMachine; }
    public TargetSensor TargetSensor { get => targetSensor; }
    public Vector3 DefaultPosition { get => defaultPosition; }
    public float ChaseSpeed { get => chaseSpeed; }
    public float DistanceBackToDefaultPosition { get => distanceBackToDefaultPosition; }

    public enum EnemyState
    {
        Idle,
        Patrol,
        Alert,
        Chase,
        Attack,
        Back,

    }

    protected virtual void Awake()
    {
        stateMachine.AddState(EnemyState.Alert, new EnemyAlertState());
        stateMachine.AddState(EnemyState.Chase, new EnemyChaseState());
        stateMachine.AddState(EnemyState.Attack, new EnemyAttackState());
        stateMachine.AddState(EnemyState.Back, new EnemyBackState());
    }

    private void Update()
    {
        stateMachine.Update(Time.deltaTime, this);
    }

    protected virtual void Reset()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
}
