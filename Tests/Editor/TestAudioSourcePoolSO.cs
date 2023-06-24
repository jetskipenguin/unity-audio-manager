using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

public class TestAudioSourcePoolSO
{
    private AudioSourcePoolSO _pool;

    [SetUp]
    public void Setup()
    {
        _pool = ScriptableObject.CreateInstance<AudioSourcePoolSO>();

        _pool._prefab = new GameObject("AudioSource").AddComponent<AudioSource>();

        _pool.Initialize();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(_pool);
    }

    [Test]
    public void Get_ReturnsActiveAudioSource()
    {
        AudioSource audioSource = _pool.Get();

        Assert.IsNotNull(audioSource);
        Assert.IsTrue(audioSource.gameObject.activeSelf);
    }

    [Test]
    public void Return_AddsAudioSourceToPool()
    {
        AudioSource audioSource = _pool.Get();

        _pool.Return(audioSource);

        Assert.IsFalse(audioSource.gameObject.activeSelf);
        Assert.Contains(audioSource, _pool._pool);
    }

    [Test]
    public void Get_CreatesNewAudioSourceIfPoolIsEmpty()
    {
        _pool._pool = new AudioSource[0];
        _pool._currentIndex = -1;

        AudioSource audioSource = _pool.Get();

        Assert.IsNotNull(audioSource);
        Assert.IsTrue(audioSource.gameObject.activeSelf);
    }
}