using System;
using UnityEngine;

[Serializable]
public class GravityForCharacterController
{
    [Tooltip("重力"), SerializeField] private float gravity;
    [Tooltip("落下速度の最大値"), SerializeField] private float maxFallSpeed = 100f;
    [Tooltip("接地判定"), SerializeField] private GroundChecker groundChecker;

    [Header("下に落ちていく速さ"), SerializeField] private float fallSpeed;
    [Header("下方向に落ちる力"), SerializeField] Vector3 fallVector;

    public Vector3 FallVector { get { return fallVector; } }

    public void Update(float deltaTime)
    {
        fallSpeed -= gravity * deltaTime;
        fallSpeed = Mathf.Clamp(fallSpeed, -maxFallSpeed, 0f);
        fallVector = new Vector3(0f, fallSpeed, 0f);
        if (groundChecker.IsGround)
        {
            fallSpeed = 0f;
            fallVector += groundChecker.GroundOffset;
        }
    }

    public void FallVectorProjectOnPlane(Vector3 plane)
    {
        fallVector = Vector3.ProjectOnPlane(fallVector, plane);
    }
}