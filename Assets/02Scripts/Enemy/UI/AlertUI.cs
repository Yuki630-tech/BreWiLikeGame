using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class AlertUI : MonoBehaviour
{
    [Tooltip("EnemyBase"), SerializeField] private EnemyBase enemyBase;
    [Tooltip("Œx‰úUI"), SerializeField] private GameObject alertUI;
    [Tooltip("“h‚è‚Â‚Ô‚·Image"), SerializeField] private Image fillAlertImage;

    private void Awake()
    {
        enemyBase.NormalizedProximityProperty.Where(x => x != -1).Subscribe(x => PaintAlertImage(x)).AddTo(gameObject);
        enemyBase.NormalizedProximityProperty.Where(x => x >= 1 || x == -1).Subscribe(_ => HideAlertImage()).AddTo(gameObject);
        alertUI.SetActive(false);
    }

    private void PaintAlertImage(float value)
    {
        alertUI.SetActive(true);
        fillAlertImage.fillAmount = value;
    }

    private void HideAlertImage()
    {
        alertUI.SetActive(false);
    }
}
