using UnityEngine;

public interface IJustAvoidable
{
    public void SetIfJustAvoidable(bool value);

    public Transform GetTargetTrans();
    public void SetTargetTrans(Transform value);
}
