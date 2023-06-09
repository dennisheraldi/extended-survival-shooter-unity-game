using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class SubtitleTrackMxer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI text = playerData as TextMeshProUGUI;
        string currentSubtitle = "";
        float currentAlpha = 0f;

        if (!text){
            return;
        }
        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);

            if (inputWeight > 0f)
            {
                ScriptPlayable<SubtitleBehaviour> inputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);
                SubtitleBehaviour input = inputPlayable.GetBehaviour();
                currentSubtitle = input.subtitleText;
                currentAlpha = inputWeight;
            }
        }

        text.text = currentSubtitle;
        text.color = new Color(1, 1, 1, currentAlpha);

    }
}
