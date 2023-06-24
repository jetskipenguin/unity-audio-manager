using UnityEngine;
using UnityEngine.Audio;

public interface IAudioConfigurationSO
{
    int Priority { get; set; }
    void ApplyTo(AudioSource audioSource);
}
