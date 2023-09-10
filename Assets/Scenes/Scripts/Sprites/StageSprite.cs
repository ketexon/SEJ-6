using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSprite : MonoBehaviour
{
    [SerializeField]
    public string Name;

    [SerializeField]
    public Image m_image;

    [SerializeField]
    public float m_fadeDuration = 0.4f;

    Animator m_animator;

    Coroutine m_fadeCoro = null;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_image.color = new Color(1, 1, 1, 0);
    }

    public void SetVariant(string variant)
    {
        if(m_animator != null && variant != null && variant.Length > 0)
        {
            m_animator.SetTrigger(variant);
        }
    }

    public void Hide()
    {
        m_fadeCoro = StartCoroutine(Fade(0.0f, () => { m_fadeCoro = null; }));
    }

    public void Show()
    {
        m_fadeCoro = StartCoroutine(Fade(1.0f, () => { m_fadeCoro = null; }));
    }

    IEnumerator Fade(float targetAlpha, System.Action callback)
    {
        float startAlpha = m_image.color.a;
        float startTime = Time.time;
        while (true)
        {
            float t = (Time.time - startTime) / m_fadeDuration;
            if(t > 1)
            {
                m_image.color = new Color(1, 1, 1, targetAlpha);
                break;
            }
            m_image.color = new Color(1, 1, 1, Mathf.Lerp(startAlpha, targetAlpha, t));
            yield return new WaitForEndOfFrame();
        }
        callback?.Invoke();
    }
}
