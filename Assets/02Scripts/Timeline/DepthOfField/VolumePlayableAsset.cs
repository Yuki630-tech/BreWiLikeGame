using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class VolumePlayableAsset : PlayableAsset, ITimelineClipAsset
{
    public float FocasDistance = 0f;
    public float FocalLength = 0f;
    public Color FilterColor;
    public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.ClipIn | ClipCaps.Extrapolation;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<VolumeClipBehaviour>.Create(graph);
        playable.GetBehaviour().FocalLength = FocalLength;
        playable.GetBehaviour().FocusDistance = FocasDistance;
        playable.GetBehaviour().FilterColor = FilterColor;
        return playable;
    }
}
