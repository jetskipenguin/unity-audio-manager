using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TNRD;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Audio control")]
	[SerializeField] internal AudioMixer _audioMixer = default;
    [Range(0f, 1f)]
    [SerializeField] internal float _masterVolume = 1f;

    [Header("Listening on channels")]
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change SFXs volume")]
	[SerializeField] internal SerializableInterface<IAudioCueEventChannelSO> _SFXEventChannel;

	[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")]
	[SerializeField] internal SerializableInterface<IAudioCueEventChannelSO> _musicEventChannel;

    [Header("Audio Source Pool")]
    [Tooltip("The pool allocates new audio sources when needed, and reuses them when they are free")]
    [SerializeField] internal SerializableInterface<IAudioSourcePoolSO> _audioSourcePool;

    private Dictionary<int, List<AudioSource>> _activeAudioCues = new Dictionary<int, List<AudioSource>>();
    private int _nextUniqueID = 0;
    
    private List<IAudioCueEventChannelSO> _audioChannels = new List<IAudioCueEventChannelSO>();
    

    void Awake()
    {
        SetGroupVolume("MasterVolume", _masterVolume);
        _audioSourcePool.Value.Initialize();

        _audioChannels.AddRange(new[]  { _SFXEventChannel.Value, _musicEventChannel.Value});
    }

    private void OnEnable()
    {
        _audioChannels.ForEach(d => d.OnAudioCuePlayRequested += PlayAudioCue);
        _audioChannels.ForEach(d => d.OnAudioCueStopRequested += StopAudioCue);
    }

    private void OnDestroy()
    {
        _audioChannels.ForEach(d => d.OnAudioCuePlayRequested -= PlayAudioCue);
        _audioChannels.ForEach(d => d.OnAudioCueStopRequested -= StopAudioCue);
    }

    public int PlayAudioCue(IAudioCueSO audioCue, IAudioConfigurationSO settings, Vector3 position = default)
	{
		List<AudioClip> clipsToPlay = audioCue.GetClips().ToList();
        List<AudioSource> sources = clipsToPlay.Select(clip => SetupAudioSource(position, clip, audioCue.looping, settings)).ToList();

        _activeAudioCues.Add(++_nextUniqueID, sources);

		return _nextUniqueID;
	}

    private AudioSource SetupAudioSource(Vector3 position, AudioClip clip, bool isLooping, IAudioConfigurationSO settings)
    {
        AudioSource source = _audioSourcePool.Value.Get();
        if (!source)
        {
            Debug.LogError("No audio source available, issue in AudioSourcePool");
            return null;
        }

        source.transform.position = position;
        settings.ApplyTo(source);
        source.clip = clip;
        source.loop = isLooping;
        source.Play();

        return source;
    }

    public bool StopAudioCue(int audioCueKey)
    {
        if (_activeAudioCues.ContainsKey(audioCueKey))
        {
            _activeAudioCues[audioCueKey].ToList().ForEach(s => _audioSourcePool.Value.Return(s));
            _activeAudioCues.Remove(audioCueKey);
            return true;
        }
        return false;
    }

    private IEnumerator ReturnAudioSource(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        _audioSourcePool.Value.Return(audioSource);
    }

    public void SetGroupVolume(string parameterName, float normalizedVolume)
	{
		bool volumeSet = _audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
		if (!volumeSet)
			Debug.LogError("The AudioMixer parameter was not found");
	}

    private float NormalizedToMixerValue(float normalizedValue)
	{
		// We're assuming the range [0 to 1] becomes [-80dB to 0dB]
		// This doesn't allow values over 0dB
		return (normalizedValue - 1f) * 80f;
	}
}