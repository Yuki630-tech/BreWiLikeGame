using UnityEngine;

public class TargetSensor : MonoBehaviour
{
    [Header("警戒モードに使うセンサーの範囲(青)")]
    [Tooltip("警戒モードに入る距離"), SerializeField] private float alertDistance = 8f;

    [Header("プレイヤーを見つけたモードに使うセンサーの範囲(赤)")]
    [Tooltip("プレイヤーを見つける距離"), SerializeField] private float chaseDistance = 5f;

    [Header("プレイヤーと対峙する距離(緑)"), SerializeField] private float strafeDistance = 3f;

    [Header("現在のセンサーのステート"), ReadOnly, SerializeField] private SensorState sensorState;
    [Header("プレイヤーとの距離"), ReadOnly, SerializeField] private float distance;

    public SensorState State { get => sensorState; }
    public float AlertDistance { get => alertDistance; }
    public float ChaseDistance { get => chaseDistance; }

    public enum SensorState
    {
        None,
        Alert,
        Chase,
        Strafe
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckTarget();
    }

    private void CheckTarget()
    {
        distance = Vector3.Distance(ComponentProvider.Instance.PlayerTrans.position, transform.position);
        if(distance <= alertDistance && distance > chaseDistance)
        {
            sensorState = SensorState.Alert;
        }

        else if(distance <= chaseDistance && distance > strafeDistance)
        {
            sensorState = SensorState.Chase;
        }

        else if(distance <= strafeDistance)
        {
            sensorState = SensorState.Strafe;
        }

        else
        {
            sensorState = SensorState.None;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, alertDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere (transform.position, strafeDistance);
    }
}
