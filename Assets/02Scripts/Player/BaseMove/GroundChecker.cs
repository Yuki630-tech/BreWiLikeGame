using UnityEngine;
using Cysharp.Threading.Tasks;
public class GroundChecker : MonoBehaviour
{
    [Header("レイの長さ、半径、始点に関する設定 Gizmosの色→赤")]
    [Tooltip("レイの始点"), SerializeField] private Vector3 startingPoint;
    [Tooltip("レイの半径"), SerializeField] private float radius;
    [Tooltip("レイの長さ"), SerializeField] private float length;
    [Tooltip("レイヤーマスク"), SerializeField] private LayerMask layerMask;
    [Tooltip("足が浮いてからこの時間の間はisGround = trueに"), SerializeField] private float groundWaitTime = 0.05f;

    [Header("接地しているか"), SerializeField] private bool isGround;
    [Header("下方向の速さを計算するか"), SerializeField] private bool isCalculateVerticalSpeed;

    private float currentTime;


    [Header("Gizmosの色→青")]
    [Tooltip("足が浮いてしまったときに最大この距離だけ下にいったところに地面があったら地面判定にする"), SerializeField] private float groundDistanceIfFloating = 0.8f;

    public Vector3 Normal { get; private set; }
    public Vector3 GroundOffset { get; private set; }
    public bool IsGround { get { return isGround; } }

    public Vector3 GroundPoint { get; private set; }
    public bool IsCalculateVerticalSpeed { get => isCalculateVerticalSpeed; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
       CheckGround();
    }
    /// <summary>
    /// 接地判定
    /// </summary>
    public void CheckGround()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + startingPoint;
        if(Physics.SphereCast(origin, radius, -transform.up, out hit, length, layerMask))
        {
            if(hit.collider.CompareTag(TagName.Ground))
            {
                isGround = true;
                isCalculateVerticalSpeed = false;
                Normal = hit.normal;
                Vector3 hitPoint = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                GroundPoint = hit.point;
                float distance = Vector3.Distance(transform.position, hitPoint);

                GroundOffset = -transform.up * (distance + radius);
            }

            else
            {
                isCalculateVerticalSpeed = true;
                currentTime += Time.deltaTime;
                if(currentTime >= groundWaitTime)
                {
                    isGround = false;
                    Normal = Vector3.zero;
                    GroundOffset = Vector3.zero;
                    GroundPoint = Vector3.zero;
                    currentTime = 0f;
                }
            }
        }

        else
        {
            isCalculateVerticalSpeed = true;
            currentTime += Time.deltaTime;
            if(currentTime >= groundWaitTime)
            {
                isGround = false;
                Normal = Vector3.zero;
                GroundOffset = Vector3.zero;
                GroundPoint = Vector3.zero;
                currentTime = 0f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + startingPoint;
        Gizmos.color = Color.black;

        Gizmos.DrawWireSphere(origin, 0.1f);
        Gizmos.DrawRay(origin, -transform.up * length);

        Vector3 endPos = origin + -transform.up * length;
        Gizmos.DrawWireSphere(endPos, radius);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, GroundOffset);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawRay(transform.position, -transform.up * groundDistanceIfFloating);

        
    }
}
