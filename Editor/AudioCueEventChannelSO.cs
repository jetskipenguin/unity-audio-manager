using UnityEngine;

/// <summary>
/// Event on which <c>AudioCue</c> components send a message to play SFX and music. <c>AudioManager</c> listens on these events, and actually plays the sound.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/Events/AudioCue Event Channel")]
public class AudioCueEventChannelSO : ScriptableObject, IAudioCueEventChannelSO
{
	public AudioCuePlayAction OnAudioCuePlayRequested {get; set;}
	public AudioCueStopAction OnAudioCueStopRequested {get; set;}
	public int RaisePlayEvent(IAudioCueSO audioCue, IAudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default)
	{
		int audioCueKey = -1;

		if (OnAudioCuePlayRequested != null)
		{
			audioCueKey = OnAudioCuePlayRequested.Invoke(audioCue, audioConfiguration, positionInSpace);
		}
		else
		{
			Debug.LogWarning("An AudioCue play event was requested, but nobody picked it up. " +
				"Check why there is no AudioManager already loaded, " +
				"and make sure it's listening on this AudioCue Event channel.");
		}

		if (audioCueKey == -1)
		{
			Debug.LogWarning("An AudioCue play event was requested, it was picked up, but had no ID returned");
		}

		return audioCueKey;
	}

	public bool RaiseStopEvent(int audioCueKey)
	{
		bool success = false;

		if (OnAudioCueStopRequested != null)
		{
			success = OnAudioCueStopRequested.Invoke(audioCueKey);
		}
		else
		{
			Debug.LogWarning("An AudioCue stop event was requested for " + audioCueKey + ", but nobody picked it up. " +
				"Check why there is no AudioManager already loaded, " +
				"and make sure it's listening on this AudioCue Event channel.");
		}

		if(!success)
		{
			Debug.LogWarning("AudioCue stop event failed for " + audioCueKey + ".");
		}

		return success;
	}
}

public delegate int AudioCuePlayAction(IAudioCueSO audioCue, IAudioConfigurationSO audioConfiguration, Vector3 positionInSpace);
public delegate bool AudioCueStopAction(int audioCueKey);
