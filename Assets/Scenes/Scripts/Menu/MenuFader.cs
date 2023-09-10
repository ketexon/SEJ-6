using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MenuFader : MonoBehaviour
{
    [SerializeField]
    float m_fadeDuration = 1.0f;

    CanvasGroup m_canvasGroup;

    void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_canvasGroup.alpha = 1;
        FadeIn();
    }

    public void FadeIn(System.Action callback = null)
    {
        StartCoroutine(FadeCoroutine(m_canvasGroup.alpha, 0, callback));
    }

    public void FadeOut(System.Action callback = null)
    {
        StartCoroutine(FadeCoroutine(m_canvasGroup.alpha, 1, callback));
    }

    IEnumerator FadeCoroutine(float from, float to, System.Action callback = null)
    {
        float startTime = Time.time;
        while (true)
        {
            float t = (Time.time - startTime) / m_fadeDuration;
            if (t > 1)
            {
                m_canvasGroup.alpha = to;
                break;
            }

            m_canvasGroup.alpha = Mathf.Lerp(from, to, t);
            yield return new WaitForEndOfFrame();
        }

        callback?.Invoke();
    }
}
