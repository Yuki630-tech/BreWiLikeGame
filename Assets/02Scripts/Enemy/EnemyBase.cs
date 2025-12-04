using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour
{
    protected StateMachine<EnemyState, EnemyBase> stateMachine = new();

    [Header("コンポーネント群")]
    [Tooltip("NavmeshAgent"), SerializeField] private NavMeshAgent navmeshAgent;
    [Tooltip("Animator"), SerializeField] private Animator animator;

    public NavMeshAgent NavmeshAgent { get => navmeshAgent; }

    public enum EnemyState
    {
        TestIdle,
        Patrol,
        Alert,
        Chase,
        Attack

    }

    protected virtual void Awake()
    {
        stateMachine.AddState(EnemyState.Alert, new EnemyAlertState());
        stateMachine.AddState(EnemyState.Chase, new EnemyChaseState());
        stateMachine.AddState(EnemyState.Attack, new EnemyAttackState());
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
