# unity-audio-manager
Reusable Audio Manager Package

## Installation

- Click Window -> Package Manager

- In top left click the plus button, then "add package from git url"

- Input the git url.

**This package has a dependency on the following package:** <br>
```
https://github.com/Thundernerd/Unity3D-SerializableInterface
```

This package can be installed via the same method.

## Scripts

Various examples can be found in: <br>
```
AudioManager/Samples/
```

<br>

### Audio Cue Scriptable Object
One audio cue represents a series of one or more audio clips that can be played sequentially or randomly <br>

These audio cues can be played via AudioEventChannel which will interface with the audiomanager. <br>

![image](https://github.com/jetskipenguin/unity-audio-manager/assets/45407009/1d45f2a0-7a6b-49bd-83e6-7d83b70bca6c)

### Audio Configuration
An audio configuration is a series of audio properties that can be reused among different audio cues. <br>
(i.e _music configuration_, _shotgun sfx configuration_, etc) <br>

![image](https://github.com/jetskipenguin/unity-audio-manager/assets/45407009/5c58322f-674a-4975-823f-8a766d6b99c7)


### Audio Source Pool
The audio source pool creates a set amount (initial size) of audio emitters that are available for the audio manager to assign audio clips too, if the pool runs out of space, new audio emitters are instantiated. <br>

The audio source pool can help save memory when there are lots of audio clips playing at a time.

![image](https://github.com/jetskipenguin/unity-audio-manager/assets/45407009/41f5c143-ef20-4b16-bcb5-3f62886f616e)


### AudioCueEventChannel
The audio event channel(s) are an intermediary between the script wanting to start an audio cue and the audio manager. <br>

Each audio event channel represents a different purpose behind the audio cue. <br>

These channels help to decouple the Audio Manager and script wanting to start an audio cue.



### Audio Manager
The audio manager listens to requests to start audio cues on AudioCueEventChannels and requests audio emitters from the AudioSourcePool to actually instantiate the sound clips. <br>

![image](https://github.com/jetskipenguin/unity-audio-manager/assets/45407009/e5a86ad0-846d-44f2-8067-575cfe71cd91)

### Playing a sound
```
using UnityEngine;

public class PlayingSoundExample : MonoBehaviour
{
	[SerializeField] private AudioCueSO _musicSO = default;
	[SerializeField] private AudioConfigurationSO _audioConfig = default;

    [SerializeField] private AudioCueEventChannelSO _musicEventChannel = default;

	private int _musicKey;

	private void Start()
	{
		PlayMusic();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
      			// stops audio cue
			_musicEventChannel.RaiseStopEvent(_musicKey);
		}
	}

	private void PlayMusic()
	{
    		// plays audio cue
		_musicKey = _musicEventChannel.RaisePlayEvent(_musicSO, _audioConfig, transform.position);
	}
}
```




