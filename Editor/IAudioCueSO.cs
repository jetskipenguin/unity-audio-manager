using UnityEngine;

public interface IAudioCueSO
{
    bool looping { get; set; }
    AudioClip[] GetClips();
}
