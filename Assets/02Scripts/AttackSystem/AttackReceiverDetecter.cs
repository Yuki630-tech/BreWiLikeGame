using UnityEngine;

public class AttackReceiverDetecter : MonoBehaviour
{
    private IJustAvoidable justAvoidable;
    private IJustGurdable justGurdable;

    public IJustAvoidable JustAvoidable { get => justAvoidable; }
    public IJustGurdable JustGurdable { get => justGurdable; }

    private void OnTriggerEnter(Collider other)
    {
        if(justAvoidable != null)
        {
            justAvoidable = other.GetComponent<IJustAvoidable>();
        }

        if(justGurdable != null)
        {
            justGurdable = other.GetComponent<IJustGurdable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        justAvoidable = null;
        justGurdable = null;
    }
}
