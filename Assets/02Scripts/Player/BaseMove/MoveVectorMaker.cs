using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[Serializable]
public class MoveVectorMaker
{
    [Tooltip("このカメラの方向に合わせて回転する"), SerializeField] private Camera playerCamera;
    [Tooltip("移動速度"), SerializeField] private float moveSpeed;
    [Tooltip("ダッシュ力"), SerializeField] private float dashPower;
    [Tooltip("回転速度"), SerializeField] private float rotateSpeed = 720f;

    [Tooltip("自分のTransform"), SerializeField] Transform transform;

    public Vector3 InputVector { get; private set; }
    public Vector3 MoveVector {  get; private set; }

    public void MakeMoveVector()
    {
        InputVector = new Vector3(InputManager.Instance.LeftStickInput.x, 0f, InputManager.Instance.LeftStickInput.y);
        var cameraRot = Quaternion.Euler(0f, playerCamera.transform.localEulerAngles.y, 0f);
        InputVector = cameraRot * InputVector;

        if(InputVector.sqrMagnitude > 0.01f)
        {
            var look = Quaternion.LookRotation(InputVector);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, look, rotateSpeed * Time.deltaTime);
        }

        MoveVector = InputManager.Instance.IsDashInput ? dashPower * moveSpeed * InputVector : moveSpeed * InputVector;
    }

}
