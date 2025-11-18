using System;
using UnityEngine;
[Serializable]
public class VerticalMoveMaker
{
    [Tooltip("重力"), SerializeField] private float gravity;
    [Tooltip("落下速度の最大値"), SerializeField] private float maxFallSpeed = 100f;
    [Tooltip("接地判定"), SerializeField] private GroundChecker groundChecker;

    [Header("下に落ちていく速さ"), SerializeField] private float verticalSpeed;
    [Header("下方向に落ちる力"), SerializeField] Vector3 fallVector;

    public Vector3 FallVector { get { return fallVector; } }

    public float VerticalSpeed { get => verticalSpeed; }

    public void Update(float deltaTime)
    {
        
        if (groundChecker.IsGround)
        {
            if(verticalSpeed <= 0f)
            {
                verticalSpeed = 0f;
            }
            
        }

        else
        {
            verticalSpeed -= gravity * deltaTime;
            verticalSpeed = Mathf.Clamp(verticalSpeed, -maxFallSpeed, maxFallSpeed);
        }

        fallVector = new Vector3(0f, verticalSpeed, 0f);
    }

    public void Jump(float jumpPower)
    {
        verticalSpeed = jumpPower;
    }

    public void FallVectorProjectOnPlane(Vector3 plane)
    {
        fallVector = Vector3.ProjectOnPlane(fallVector, plane);
    }
}
