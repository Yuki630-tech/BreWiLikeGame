using System;
using UniRx;
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
    [Tooltip("入力の方向を向くかどうか"), SerializeField] private bool isTurnToInput;
    [Tooltip("カメラの方向を向くかどうか"), SerializeField] private bool isTurnToCamera;

    [Header("プレイヤーの今の速度"), ReadOnly, SerializeField] private float speed;

    [Tooltip("自分のTransform"), SerializeField] Transform transform;

    public Vector3 InputVector { get; private set; }
    public Vector3 MoveVector {  get; private set; }


    public float Speed { get => speed; }
    public float MoveSpeed { get => moveSpeed; }
    public float DashPower { get => dashPower; }

    public void MakeMoveVector()
    {
        InputVector = new Vector3(InputManager.Instance.LeftStickInput.x, 0f, InputManager.Instance.LeftStickInput.y);
        var cameraRot = Quaternion.Euler(0f, playerCamera.transform.localEulerAngles.y, 0f);
        InputVector = isTurnToInput ? cameraRot * InputVector : InputVector;

        if(InputVector.sqrMagnitude > 0.01f && isTurnToCamera)
        {
            var look = isTurnToInput ? Quaternion.LookRotation(InputVector) : cameraRot;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, look, rotateSpeed * Time.deltaTime);
        }

        MoveVector = InputManager.Instance.IsDashInput ? dashPower * moveSpeed * InputVector : moveSpeed * InputVector;
        if(InputVector.magnitude > 0f)
        {
            speed = InputManager.Instance.IsDashInput ? dashPower * moveSpeed : moveSpeed;
        }

        else
        {
            speed = 0f;
        }
    }

    public void SetIfTurnToCamera(bool value)
    {
        isTurnToCamera = value;
    }

}
