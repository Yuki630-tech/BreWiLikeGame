using UnityEngine;

public interface IJustAvoidable
{
    public Transform GetCounterTrans();

    public void SetCounterTrans(Transform setCounterPos);
    public void SetIfJustAvoidable(bool value);
}
