using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CameraPlayableAsset : PlayableAsset, ITimelineClipAsset
{
    [SerializeField] private CameraPlayableBehaviour cameraPlayableBehaviour = new();
    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<CameraPlayableBehaviour>.Create(graph, cameraPlayableBehaviour);
    }
}
