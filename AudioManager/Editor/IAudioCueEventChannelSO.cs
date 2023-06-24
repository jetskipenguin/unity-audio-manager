using UnityEngine;

public interface IAudioCueEventChannelSO
{
    AudioCuePlayAction OnAudioCuePlayRequested {get; set;}
	AudioCueStopAction OnAudioCueStopRequested {get; set;}

    int RaisePlayEvent(IAudioCueSO audioCue, IAudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default);
    bool RaiseStopEvent(int audioCueKey);
}
