using System.Diagnostics;
using UniRx;
using UnityEngine;

public class TargetMarkerSpawner : MonoBehaviour
{
    [Tooltip("マーカー"), SerializeField] private GameObject targetMarker;

    private void Awake()
    {
        ComponentProvider.Instance.EnemyDetecter.TargetEnemy.Where(x => x == gameObject && InputManager.Instance.IsShieldPushing).Subscribe(_ => SetTarget(true)).AddTo(gameObject);
        ComponentProvider.Instance.EnemyDetecter.TargetEnemy.Where(x => x != gameObject).Subscribe(_ => SetTarget(false)).AddTo(gameObject);
        targetMarker.SetActive(false);
    }
    public void SetTarget(bool isTarget)
    {
        targetMarker.SetActive(isTarget);
    }
}
