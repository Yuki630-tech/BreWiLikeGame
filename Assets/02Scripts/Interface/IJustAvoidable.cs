using UnityEngine;

public interface IJustAvoidable
{
    public void SetIfJustAvoidable(bool value);

    public Transform GetTargetTrans();
    public void SetTargetTrans(Transform value);

    public Transform GetEnemyTrans();

    public void SetEnemyTrans(Transform value);
}
