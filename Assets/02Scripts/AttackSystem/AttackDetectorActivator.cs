using System.Collections.Generic;
using UnityEngine;

public class AttackDetectorActivator : MonoBehaviour
{
    [Tooltip("AttackDetector‚ÌƒŠƒXƒg"), SerializeField] private List<AttackDetector> attackDetectorList = new();

    public void ActivateAttackDetector(string attackDetectorName)
    {
        AttackDetector attackDetector = attackDetectorList.Find(x => x.gameObject.name == attackDetectorName);
        attackDetector.gameObject.SetActive(true);
    }
}
