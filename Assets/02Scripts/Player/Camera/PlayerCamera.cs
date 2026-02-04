using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Cinemachine;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
public class PlayerCamera : MonoBehaviour
{
    [Tooltip("CinemachineCamraのデータのリスト"), SerializeField] private List<CameraData> cameraDataList = new();
    [Tooltip("PlayerのTransform"), SerializeField] private Transform playerTrans;
    [Tooltip("TargetGroup"), SerializeField] private CinemachineTargetGroup targetGroup;
    [Tooltip("アクティブなCinemachineCamraの優先度"), SerializeField] private int activeCameraPriority = 10;
    [Tooltip("敵の方向に向く速度"), SerializeField] private float rotSpeed = 2f;
    [Tooltip("非アクティブなCinemachineCameraの優先度"), SerializeField] private int deActiveCameraPriority = -1;
    [ReadOnly, SerializeField] private CameraKind currentCameraKind;

    [ReadOnly, SerializeField] private float goalValue;

    private CancellationTokenSource cts = new();

    public float RotSpeed { get => rotSpeed; }

    public enum CameraKind
    {
        Player,
        TargetGroup
    }

    [System.Serializable]
    private class CameraData
    {
        public CameraKind CameraKind;
        public CinemachineCamera CinemachineCamra;
    }

    private void Awake()
    {
        currentCameraKind = cameraDataList.Find(x => x.CinemachineCamra.Priority == activeCameraPriority).CameraKind;
        ComponentProvider.Instance.EnemyDetecter.TargetEnemy.Where(x => x != null && InputManager.Instance.IsShieldPushing).Subscribe(x =>
        {
            SetCamera(false, CameraKind.TargetGroup);
            SetSecondTarget(x.transform);
            _ = LookAt(CameraKind.TargetGroup, playerTrans, x.transform, rotSpeed);
        }).AddTo(gameObject);

        ComponentProvider.Instance.EnemyDetecter.TargetEnemy.Where(x => x == null).Subscribe(_ => SetCamera(true, CameraKind.Player)).AddTo(gameObject);
    }

    public void InitializeCurrentCamera()
    {
        currentCameraKind = cameraDataList.Find(x => x.CinemachineCamra.Priority == activeCameraPriority).CameraKind;
    }

    private void OnEnable()
    {
        cts = new();
    }

    private void OnDisable()
    {
        cts.Cancel();
    }
    public void SetCamera(bool isInherited, CameraKind setKind)
    {
        CinemachineCamera cinemachineCamera = cameraDataList.Find(x => x.CameraKind == setKind).CinemachineCamra;
        Debug.Log(cinemachineCamera.name);
        CinemachineCamera currentCinemachine = cameraDataList.Find(x => x.CameraKind == currentCameraKind).CinemachineCamra;

        CinemachineOrbitalFollow cinemachineOrbitalFollow = cinemachineCamera.GetComponent<CinemachineOrbitalFollow>();
        CinemachineOrbitalFollow currentCinemachineOrbitalFollow = currentCinemachine.GetComponent<CinemachineOrbitalFollow>();
        bool bothHaveOrbitalFollow = cinemachineOrbitalFollow != null && currentCinemachine != null;

        if (isInherited && bothHaveOrbitalFollow)
        {
            float currentHorizontalValue = currentCinemachineOrbitalFollow.HorizontalAxis.Value;
            float currentVerticalValue = currentCinemachineOrbitalFollow.VerticalAxis.Value;

            cinemachineOrbitalFollow.HorizontalAxis.Value = currentHorizontalValue;
            cinemachineOrbitalFollow.VerticalAxis.Value = currentVerticalValue;
        }
        cinemachineCamera.Priority = activeCameraPriority;
        currentCinemachine.Priority = deActiveCameraPriority;
        currentCameraKind = setKind;

    }

    public void SetSecondTarget(Transform setTarget)
    {
        targetGroup.Targets[1].Object = setTarget;

    }

    public async UniTask LookAt(CameraKind lookCameraKind, Transform startTrans, Transform lookAtTrans, float rotSpeed)
    {
        try
        {
            CinemachineCamera cinemachineCamera = cameraDataList.Find(x => x.CameraKind == lookCameraKind).CinemachineCamra;
            CinemachineOrbitalFollow cinemachineOrbitalFollow = cinemachineCamera.GetComponent<CinemachineOrbitalFollow>();
            float horizontalAxisValue = cinemachineOrbitalFollow.HorizontalAxis.Value;
            Vector3 direction = (lookAtTrans.position - startTrans.position).normalized;
            float angle = Vector3.Angle(Vector3.forward, direction);
            goalValue = direction.x > 0 ? angle : -angle;
            while (Mathf.Abs(horizontalAxisValue - goalValue) > 5f)
            {
                horizontalAxisValue = Mathf.Lerp(horizontalAxisValue, goalValue, rotSpeed * Time.deltaTime);
                cinemachineOrbitalFollow.HorizontalAxis.Value = horizontalAxisValue;
                await UniTask.Yield();
            }
        }

        catch
        {

        }



    }
    
}
