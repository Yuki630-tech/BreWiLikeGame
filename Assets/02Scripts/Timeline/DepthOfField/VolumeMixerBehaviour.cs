using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
[System.Serializable]
public class VolumeMixerBehaviour : PlayableBehaviour
{
    private DepthOfField depthOfField;
    private ColorAdjustments colorAdjustments;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var volume = playerData as Volume;

        if (volume == null) return;

        if(depthOfField == null)
        {
            volume.profile.TryGet(out depthOfField);
            if (depthOfField == null) return;
        }

        if(colorAdjustments == null)
        {
            volume.profile.TryGet(out colorAdjustments);
            if (colorAdjustments == null) return;
        }

        float blendedFocasDistance = 0f;
        float blendedFocalLength = 0f;
        Color blendedFilterColor = Color.black;
        float totalWeight = 0f;

        int inputCount = playable.GetInputCount();

        for(int i = 0; i < inputCount; i++)
        {
            float weight = playable.GetInputWeight(i);
            if (weight <= 0f) continue;

            var inputPlayable = (ScriptPlayable<VolumeClipBehaviour>)playable.GetInput(i);
            var input = inputPlayable.GetBehaviour();

            blendedFocasDistance += input.FocusDistance * weight;
            blendedFocalLength += input.FocalLength * weight;
            blendedFilterColor += input.FilterColor * weight;
            totalWeight += weight;
        }

        if(totalWeight > 0f)
        {
            depthOfField.focusDistance.value = blendedFocasDistance / totalWeight;
            depthOfField.focalLength.value = blendedFocalLength / totalWeight;
            colorAdjustments.colorFilter.value = blendedFilterColor / totalWeight;
        }
    }
}
