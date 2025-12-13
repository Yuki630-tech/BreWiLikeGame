using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(TargetSensor))]
public class EnemyBase : MonoBehaviour
{
    protected StateMachine<EnemyState, EnemyBase> stateMachine = new();
    protected EnemyAttackStrategyFactory enemyAttackStrategyFactory = new();

    [Header("コンポーネント群")]
    [Tooltip("NavmeshAgent"), SerializeField] private NavMeshAgent navmeshAgent;
    [Tooltip("Animator"), SerializeField] private Animator animator;
    [Tooltip("TargetSensor"), SerializeField] private TargetSensor targetSensor;

    [Header("Chaseステートに関する設定")]
    [Tooltip("Chase中の移動スピード"), SerializeField] private float chaseSpeed = 5f;
    [Tooltip("発見した時に表示させるUI"), SerializeField] private GameObject chaseUI;
    [Tooltip("発見UIを表示させる時間"), SerializeField] private float showUITime = 0.8f;
    [Tooltip("拠点に戻る距離"), SerializeField] private float distanceBackToDefaultPosition = 10f;

    [Header("敵のスタート地点"), ReadOnly, SerializeField] protected Vector3 defaultPosition;
    [Header("Alertステートに関する設定")]
    [Header("警戒に入る最大距離に対してどれだけ近づいているか"), ReadOnly, SerializeField]
    private ReactiveProperty<float> normalizedProximityProperty = new();

    private CancellationTokenSource cts;

    public IReadOnlyReactiveProperty<float> NormalizedProximityProperty => normalizedProximityProperty;

    public NavMeshAgent NavmeshAgent { get => navmeshAgent; }
    public Animator Animator { get => animator; }
    public StateMachine<EnemyState, EnemyBase> StateMachine { get => stateMachine; }
    public TargetSensor TargetSensor { get => targetSensor; }
    public Vector3 DefaultPosition { get => defaultPosition; }
    public float ChaseSpeed { get => chaseSpeed; }
    public float DistanceBackToDefaultPosition { get => distanceBackToDefaultPosition; }
    public EnemyAttackStrategyFactory EnemyAttackStrategyFactory { get => enemyAttackStrategyFactory; }

    public enum EnemyState
    {
        Idle,
        Patrol,
        Alert,
        Chase,
        Strafe,
        Attack,
        Back,

    }

    protected virtual void Awake()
    {
        normalizedProximityProperty.Value = -1;
        stateMachine.AddState(EnemyState.Alert, new EnemyAlertState());
        stateMachine.AddState(EnemyState.Chase, new EnemyChaseState());
        stateMachine.AddState(EnemyState.Attack, new EnemyAttackState());
        stateMachine.AddState(EnemyState.Back, new EnemyBackState());

    }

    private void OnEnable()
    {
        cts = new CancellationTokenSource();    
    }

    private void OnDisable()
    {
        cts.Cancel();
    }

    private void Update()
    {
        stateMachine.Update(Time.deltaTime, this);
    }

    public void SetNormalizedProximity(float distance)
    {
        normalizedProximityProperty.Value = (targetSensor.AlertDistance - distance) / (targetSensor.AlertDistance - targetSensor.ChaseDistance);
    }

    public void SetNormalizedProximityToEndValue()
    {
        normalizedProximityProperty.Value = -1;
    }

    protected virtual void Reset()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public async UniTask ShowChaseUITask()
    {
        try
        {
            chaseUI.SetActive(true);
            await UniTask.Delay(System.TimeSpan.FromSeconds(showUITime), cancellationToken: cts.Token);
            chaseUI.SetActive(false);
        }

        catch (OperationCanceledException)
        {

        }
    }
}
