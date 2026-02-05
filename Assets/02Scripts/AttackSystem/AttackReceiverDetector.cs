using UnityEngine;

public class AttackReceiverDetector : MonoBehaviour
{
    [Tooltip("敵のTransform"), SerializeField] private Transform enemyTrans;
    private IJustAvoidable justAvoidable;
    private IJustGurdable justGurdable;

    public IJustAvoidable JustAvoidable { get => justAvoidable; }
    public IJustGurdable JustGurdable { get => justGurdable; }

    private void Update()
    {
        //Debug.Log("JustAvoidable : " + justAvoidable != null);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IJustAvoidable>() != null)
        {
            //Debug.Log("ジャスト回避する奴が入ってきた");
            justAvoidable = other.GetComponent<IJustAvoidable>();
            justAvoidable.SetTargetTrans(transform);
            justAvoidable.SetEnemyTrans(enemyTrans);
        }

        if(other.GetComponent<IJustGurdable>() != null)
        {
            //Debug.Log("ジャストガードする奴が入ってきた");
            justGurdable = other.GetComponent<IJustGurdable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        justAvoidable = null;
        justGurdable = null;
    }
}
