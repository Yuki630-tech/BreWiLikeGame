using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class CameraPlayableBehaviour : PlayableBehaviour
{
    [SerializeField] private LayerMask cullingMask;
    [SerializeField] private CameraClearFlags clearFlags;

    [ReadOnly, SerializeField] private Camera cam;
    private void Init(object playerData)
    {
        if(cam == null)
        {
            cam = playerData as Camera;
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Init(playerData);

        if(cam != null)
        {
            cam.cullingMask = cullingMask;
            cam.clearFlags = clearFlags;
        }
        else
        {
            Debug.LogError("ÉJÉÅÉâÇ™ë∂ç›ÇµÇ‹ÇπÇÒ");
        }
    }
}
