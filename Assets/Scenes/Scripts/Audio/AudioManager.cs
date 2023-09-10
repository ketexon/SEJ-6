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

    [SerializeField]
    AudioSource m_audioSourceFront;

    [SerializeField]
    AudioSource m_audioSourceBack;

    [SerializeField]
    float m_fadeDuration = 5f;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"Multiple MusicManagers added. Destroying {this}...");
            Destroy(this);
            return;
        }

        Instance = this;
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
        float time = m_audioSourceFront.time;
        m_audioSourceBack.clip = clip;
        m_audioSourceBack.volume = volume;
        m_audioSourceBack.time = time;
        m_audioSourceBack.Play();

        IEnumerator FadeCoroutine()
        {
            float startTime = Time.time;
            while (true)
            {
                float t = (Time.time - startTime) / m_fadeDuration;
                if(t > 1)
                {
                    break;
                }

                m_audioSourceBack.volume = t;
                m_audioSourceFront.volume = 1 - t;
                yield return new WaitForEndOfFrame();
            }
            m_audioSourceBack.volume = 1.0f;
            m_audioSourceFront.volume = 0.0f;
            // swap the two *references*, as to not disturb the audio already playing
            (m_audioSourceFront, m_audioSourceBack) = (m_audioSourceBack, m_audioSourceFront);
            m_audioSourceBack.Stop();
        }
        StartCoroutine(FadeCoroutine());
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
        m_audioSourceFront.PlayOneShot(clip, volume);
    }
}
