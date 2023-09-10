using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueSceneEntrance : MonoBehaviour
{
    [SerializeField]
    DialogueRunner m_dialogueRunner;

    [SerializeField]
    string m_startDialogue = "Start";

    [SerializeField]
    CanvasGroup m_blackScreen;

    [SerializeField]
    float m_fadeDuration = 1f;

    void Start()
    {
        IEnumerator FadeInCoroutine()
        {
            float startTime = Time.time;
            bool startedDialogue = false;
            while (true)
            {
                float t = (Time.time - startTime) / m_fadeDuration;
                if(t > 0.5f && !startedDialogue)
                {
                    m_dialogueRunner.StartDialogue(m_startDialogue);
                    startedDialogue = true;
                }
                if(t > 1)
                {
                    m_blackScreen.alpha = 0;
                    break;
                }
                m_blackScreen.alpha = 1 - t;
                yield return new WaitForEndOfFrame();
            }
        }

        StartCoroutine(FadeInCoroutine());
    }
}
