using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    enum State
    {
        FadeInText,
        TextFadedIn,
        FadeInHome,
        HomeFadedIn,
        FadeOutText,
        TextFadedOut,
        FadeOutHome,
        HomeFadedOut,
    }

    [SerializeField]
    CanvasGroup m_textStart;

    [SerializeField]
    CanvasGroup m_textMiddle;

    [SerializeField]
    CanvasGroup m_textEnd;

    [SerializeField]
    string m_nextScene = "Menu";

    [SerializeField]
    float m_fadeTextDuration = 1.0f;

    [SerializeField]
    float m_textDurationIntro = 2.0f;

    [SerializeField]
    float m_textDurationOutro = 4.0f;

    [SerializeField]
    float m_fadeInHomeDuration = 2.0f;

    [SerializeField]
    float m_fadeOutHomeDuration = 2.0f;

    [SerializeField]
    float m_homeDuration = 5.0f;

    [SerializeField]
    float m_fadeInHomeOffset = 1.0f;

    float m_startTime;

    int m_state = 0;

    void Start()
    {
        m_startTime = Time.time;

        m_textStart.alpha = 0;
        m_textMiddle.alpha = 0;
        m_textEnd.alpha = 0;


        float m_textDuration = GlobalState.SecretSeen ? m_textDurationOutro : m_textDurationIntro;

        StartCoroutine(Fade(new CanvasGroup[] { m_textStart, m_textEnd }, 0, 1, m_fadeTextDuration, () =>
        {
            StartCoroutine(Wait(m_textDuration, () =>
            {
                StartCoroutine(Fade(new CanvasGroup[] { m_textStart, m_textEnd }, 1, 0, m_fadeTextDuration, () =>
                {
                    if(!GlobalState.SecretSeen)
                    {
                        SceneManager.LoadScene(m_nextScene);
                    }
                }));
            }));
        }));

        if (GlobalState.SecretSeen)
        {
            StartCoroutine(Wait(m_fadeInHomeOffset, () =>
            {
                StartCoroutine(Fade(m_textMiddle, 0, 1, m_fadeInHomeDuration, () =>
                {
                    StartCoroutine(Wait(m_homeDuration, () =>
                    {
                        StartCoroutine(Fade(m_textMiddle, 1, 0, m_fadeOutHomeDuration, null));
                    }));
                }));
            }));
        }
    }

    IEnumerator Fade(CanvasGroup group, float from, float to, float duration, System.Action callback)
    {
        return Fade(new CanvasGroup[] { group }, from, to, duration, callback);
    }

    IEnumerator Fade(CanvasGroup[] groups, float from, float to, float duration, System.Action callback)
    {
        float startTime = Time.time;
        while (true)
        {
            float t = (Time.time - startTime) / duration;
            if(t > 1)
            {
                break;
            }
            foreach (var g in groups)
            {
                g.alpha = Mathf.Lerp(from, to, t);
            }
            yield return new WaitForEndOfFrame();
        }
        foreach (var g in groups)
        {
            g.alpha = to;
        }
        callback?.Invoke();
    }

    IEnumerator Wait(float duration, System.Action callback)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke();
    }

    //void Update()
    //{
    //    if (!GlobalState.SecretSeen)
    //    {
    //        float t = Time.time - m_startTime;
    //        if (t < 1)
    //        {
    //            m_textStart.alpha = t;
    //            m_textEnd.alpha = t;
    //        }
    //        else if (t < 3)
    //        {
    //            m_textStart.alpha = 1;
    //            m_textEnd.alpha = 1;
    //        }
    //        else if (t > 3)
    //        {
    //            m_textStart.alpha = Mathf.Max(4 - t, 0);
    //            m_textEnd.alpha = Mathf.Max(4 - t, 0);
    //        }
    //        if (t > 4)
    //        {
    //            SceneManager.LoadScene(m_nextScene);
    //        }
    //    }
    //    else
    //    {
    //        float t = Time.time - m_startTime;
    //        if (m_state == 0)
    //        {
                
    //        }
    //        if (t < 1f)
    //        {
    //            m_textMiddle.alpha = 0;
    //        }
    //        else if (t > 1 && t < )
    //        {
    //            m_textMiddle.alpha = t - 0.5f;
    //        }
    //        else if (t < 4)
    //        {
    //            m_textMiddle.alpha = 1;
    //        }
    //        else if (t > 4)
    //        {
    //            m_textMiddle.alpha = Mathf.Max(5 - t, 0);
    //        }
    //    }
    //}
}
