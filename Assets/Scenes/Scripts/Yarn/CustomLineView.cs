using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using System;

public class CustomLineView : DialogueViewBase
{
#if UNITY_EDITOR
    const bool AllowSkipAudio = true;
#else
    const bool AllowSkipAudio = false;
#endif

    public static CustomLineView Instance;


    [SerializeField]
    CanvasGroup m_canvasGroup;

    [SerializeField]
    TMP_Text m_characterName;

    [SerializeField]
    TMP_Text m_text;

    [SerializeField]
    AudioSource m_audioSource;

    [SerializeField]
    float m_typeWriterSpeed = 30.0f;

    Coroutine m_presentCoroutine = null;

    void Reset()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_audioSource = GetComponent<AudioSource>();
    }

    void Awake()
    {
        m_canvasGroup.alpha = 0;
        m_canvasGroup.blocksRaycasts = false;
        Instance = this;
    }

    public override void DismissLine(Action onDismissalComplete)
    {
        m_canvasGroup.blocksRaycasts = false;
        m_text.text = "";
        m_characterName.text = "";

        onDismissalComplete?.Invoke();
    }

    public override void InterruptLine(LocalizedLine dialogueLine, Action onInterruptLineFinished)
    {
        PresentLine();
        onInterruptLineFinished?.Invoke();
    }

    public override void UserRequestedViewAdvancement()
    {
        if (m_audioSource.isPlaying && !AllowSkipAudio)
        {
            Debug.Log("Still playing audio...");
            return;
        }
        if(m_presentCoroutine != null)
        {
            PresentLine();
        }
        else
        {
            m_audioSource.Stop();
            requestInterrupt?.Invoke();
        }
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        m_characterName.text = dialogueLine.CharacterName;
        m_text.text = dialogueLine.TextWithoutCharacterName.Text;
        m_canvasGroup.alpha = 1;
        m_canvasGroup.blocksRaycasts = true;

        if (dialogueLine.Asset is AudioClip clip)
        {
            m_audioSource.clip = clip;
            m_audioSource.Play();
        }

        IEnumerator PresentCoro()
        {
            float startTime = Time.time;
            m_text.maxVisibleCharacters = 0;
            yield return new WaitForEndOfFrame();
            while (m_text.maxVisibleCharacters < m_text.text.Length)
            {
                m_text.maxVisibleCharacters = (int)((Time.time - startTime) * m_typeWriterSpeed);
                yield return new WaitForEndOfFrame();
            }
            PresentLine();
        }

        m_presentCoroutine = StartCoroutine(PresentCoro());
    }

    void PresentLine()
    {
        if (m_presentCoroutine != null)
        {
            StopCoroutine(m_presentCoroutine);
            m_presentCoroutine = null;
        }

        m_text.maxVisibleCharacters = m_text.text.Length;

        m_canvasGroup.blocksRaycasts = true;
    }

    [YarnCommand("hide_dialogue")]
    public static void HideDialogue()
    {
        Instance.m_canvasGroup.alpha = 0;
    }
}
