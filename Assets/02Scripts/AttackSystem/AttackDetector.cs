using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AttackDetector : MonoBehaviour
{
    [Tooltip("攻撃力等のデータ"), SerializeField] private AttackData attackData;
    [Tooltip("表示させておく時間"), SerializeField] private float attackInterval = 0.3f;

    [Header("一度ダメージを受けたAttackReceiverのリスト"), SerializeField] private List<AttackReceiver> receiverList = new();

    private CancellationTokenSource cts = new();

    private async void OnEnable()
    {
        try
        {
            receiverList.Clear();

            await UniTask.Delay(System.TimeSpan.FromSeconds(attackInterval), cancellationToken:cts.Token);
            gameObject.SetActive(false);
        }
        catch (OperationCanceledException)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AttackReceiver attackReceiver = other.GetComponent<AttackReceiver>();

        if(attackReceiver != null && !receiverList.Contains(attackReceiver))
        {
            attackReceiver.OnAttackReceived(this);
        }
    }
}
