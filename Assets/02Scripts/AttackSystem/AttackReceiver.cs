using UnityEngine;
using UnityEngine.Events;

public class AttackReceiver : MonoBehaviour
{
    [Tooltip("ダメージを受けた際に起こすイベントを登録する"), SerializeField] private UnityEvent<AttackDetector> onAttackReceived = new();
    public void OnAttackReceived(AttackDetector detecter)
    {
        onAttackReceived.Invoke(detecter);
    }
}
