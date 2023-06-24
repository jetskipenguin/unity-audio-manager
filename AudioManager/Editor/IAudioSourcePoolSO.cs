using UnityEngine;

public interface IAudioSourcePoolSO
{
    void Initialize();
    AudioSource Get();
    void Return(AudioSource obj);
}