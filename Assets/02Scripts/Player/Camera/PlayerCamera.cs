using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [Tooltip("CinemachineCamraのデータのリスト"), SerializeField] private List<CameraData> cameraDataList = new();
    [Tooltip("TargetGroup"), SerializeField] private CinemachineTargetGroup targetGroup;
    [Tooltip("アクティブなCinemachineCamraの優先度"), SerializeField] private int activeCameraPriority = 10;
    [Tooltip("非アクティブなCinemachineCameraの優先度"), SerializeField] private int deActiveCameraPriority = -1;
    [ReadOnly, SerializeField] private CameraKind currentCameraKind;
    public enum CameraKind
    {
        Player,
        TargetGroup
    }

    private void Awake()
    {
        currentCameraKind = cameraDataList.Find(x => x.CinemachineCamra.Priority == activeCameraPriority).CameraKind;
    }

    [System.Serializable]
    private class CameraData
    {
        public CameraKind CameraKind;
        public CinemachineCamera CinemachineCamra;
    }

    public void SetCamera(CameraKind setKind)
    {
        CinemachineCamera cinemachineCamera = cameraDataList.Find(x => x.CameraKind == setKind).CinemachineCamra;
        Debug.Log(cinemachineCamera.name);
        CinemachineCamera currentCinemachine = cameraDataList.Find(x => x.CameraKind == currentCameraKind).CinemachineCamra;

        cinemachineCamera.Priority = activeCameraPriority;
        currentCinemachine.Priority = deActiveCameraPriority;
        currentCameraKind = setKind;

    }

    public void SetSecondTarget(Transform setTarget)
    {
        targetGroup.Targets[1].Object = setTarget;
    }
    
}
