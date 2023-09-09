using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    AudioDatabase m_audioDatabase;

    AudioSource m_audioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"Multiple MusicManagers added. Destroying {this}...");
            Destroy(this);
            return;
        }

        Instance = this;

        m_audioSource = GetComponent<AudioSource>();
    }

    void OnDestroy()
    {
        if(Instance != null)
        {
            Instance = null;
        }
    }

    [YarnCommand("set_music")]
    public static void SetMusic(string name, float volume = 1)
    {
        Instance.SetMusicImpl(name, volume);
    }

    void SetMusicImpl(string name, float volume)
    {
        AudioClip clip = m_audioDatabase.Get(name);
        if(clip == null)
        {
            Debug.LogError($"Cannot find music with name: {name}");
            return;
        }
        m_audioSource.clip = clip;
        m_audioSource.volume = volume;
        m_audioSource.Play();
    }

    [YarnCommand("play_sfx")]
    public static void PlaySFX(string name, float volume = 1)
    {
        Instance.PlaySFXImpl(name, volume);
    }

    void PlaySFXImpl(string name, float volume)
    {
        AudioClip clip = m_audioDatabase.Get(name);
        if (clip == null)
        {
            Debug.LogError($"Cannot find music with name: {name}");
            return;
        }
        m_audioSource.PlayOneShot(clip, volume);
    }
}
