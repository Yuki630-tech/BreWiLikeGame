using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Rendering;

//処理に使用する情報のノードと実際の処理を命令するノードとを作成する
[TrackBindingType(typeof(Volume))] //処理によって変化させるオブジェクトの型(トラックの一番左に設定する変数の型)
[TrackClipType(typeof(VolumePlayableAsset))] //クリップが保持している情報ノードのタイプを決定
public class VolumeTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<VolumeMixerBehaviour>.Create(graph, inputCount); //ミックス処理を行う処理ノードを作成
    }
}
