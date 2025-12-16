using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class VolumePlayableAsset : PlayableAsset, ITimelineClipAsset
{
    public float FocusDistance = 0f;
    public float FocalLength = 0f;
    public Color FilterColor;
    public ClipCaps clipCaps => ClipCaps.Blending | ClipCaps.ClipIn | ClipCaps.Extrapolation;

    //タイムライン上でマイフレーム作成されるノード(Playable)
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<VolumeClipBehaviour>.Create(graph); //VolumeClipBehaviour型のノード

        //
        playable.GetBehaviour().FocalLength = FocalLength;
        playable.GetBehaviour().FocusDistance = FocusDistance;
        playable.GetBehaviour().FilterColor = FilterColor;
        return playable;
    }
}
