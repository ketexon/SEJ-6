using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    [SerializeField]
    CanvasGroup m_text;

    [SerializeField]
    string m_nextScene = "Menu";

    float m_startTime;
    void Start()
    {
        m_startTime = Time.time;
    }

    void Update()
    {
        float t = Time.time - m_startTime;
        if (t < 1)
        {
            m_text.alpha = t;
        }
        else if(t < 3)
        {
            m_text.alpha = 1;
        }
        else if(t > 3)
        {
            m_text.alpha = Mathf.Max(4 - t, 0);
            if(t > 4)
            {
                SceneManager.LoadScene(m_nextScene);
            }
        }
    }
}
