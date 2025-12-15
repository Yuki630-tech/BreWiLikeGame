using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Rendering;

[TrackBindingType(typeof(Volume))]
[TrackClipType(typeof(VolumePlayableAsset))]
public class VolumeFieldTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<VolumeMixerBehaviour>.Create(graph, inputCount);
    }
}
