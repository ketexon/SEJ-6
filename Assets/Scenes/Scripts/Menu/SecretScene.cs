using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class SecretScene : MonoBehaviour
{
    public static SecretScene Instance;

    [SerializeField]
    PostProcessVolume m_chromaticAbberrationVolume;

    [SerializeField]
    Vector2 m_chromaticAbberrationFlickerIntervalRange = new Vector2(0.5f, 2);

    [SerializeField]
    Vector2 m_chromaticAbberrationFlickerIntensityRange = new Vector2(0.1f, 1);

    [SerializeField]
    float m_chromaticAbberrationDuration = 0.1f;

    [SerializeField]
    MenuFader m_menuFader;

    [SerializeField]
    string m_outroScene = "Intro";

    [SerializeField]
    DialogueRunner m_dialogueRunner;

    [SerializeField]
    string m_secretScript = "Secret";

    [SerializeField]
    float m_secretScriptWaitTime = 5;

    [SerializeField]
    CanvasGroup m_filledBackground;

    [SerializeField]
    float m_filledBackgroundFadeDuration = 1f;

    [SerializeField]
    CanvasGroup m_background;

    bool sceneStarted = false;

    ChromaticAberration m_chromaticAbberration;
    Coroutine m_chromaticAbberrationCoroutine = null;

    float m_chromaticAbberrationIntensityMult = 1.0f;

    void Awake()
    {
        Instance = this;

        m_background.alpha = 0;
        m_filledBackground.alpha = 0;
        m_chromaticAbberration = m_chromaticAbberrationVolume.profile.GetSetting<ChromaticAberration>();
    }

    public void StartScene()
    {
        GlobalState.SecretSeen = true;

        m_background.alpha = 1.0f;

        m_chromaticAbberrationCoroutine = StartCoroutine(ChromaticAbberrationCoroutine());

        IEnumerator StartDialogueCoroutine()
        {
            yield return new WaitForSeconds(m_secretScriptWaitTime);
            m_dialogueRunner.StartDialogue(m_secretScript);
        }

        StartCoroutine(StartDialogueCoroutine());
    }

    //void Update()
    //{
    //    if(sceneStarted)
    //    {
            
    //    }
    //}

    IEnumerator ChromaticAbberrationCoroutine()
    {
        while (true)
        {
            float wait = Random.Range(
                m_chromaticAbberrationFlickerIntervalRange.x,
                m_chromaticAbberrationFlickerIntervalRange.y
            );
            yield return new WaitForSeconds(wait);
            float intensity = Random.Range(
                m_chromaticAbberrationFlickerIntensityRange.x,
                m_chromaticAbberrationFlickerIntensityRange.y
            );
            m_chromaticAbberration.intensity.value = intensity * m_chromaticAbberrationIntensityMult;
            m_chromaticAbberration.enabled.value = true;
            yield return new WaitForSeconds(m_chromaticAbberrationDuration);
            m_chromaticAbberration.enabled.value = false;

            if (Mathf.Approximately(m_chromaticAbberrationIntensityMult, 0.0f)) break;
        }
    }

    [YarnCommand("goto_outro")]
    public static void GotoOutro()
    {
        Instance.GotoOutroImpl();
    }

    void GotoOutroImpl()
    {
        m_menuFader.FadeOut(() =>
        {
            SceneManager.LoadScene(m_outroScene);
        });
    }

    [YarnCommand("show_color")]
    public static void ShowColor()
    {
        Instance.ShowColorImpl();
    }

    void ShowColorImpl()
    {
        IEnumerator FadeCoroutine()
        {
            float startTime = Time.time;
            while (true)
            {
                float t = (Time.time - startTime) / m_filledBackgroundFadeDuration;
                if (t > 1) break;
                m_filledBackground.alpha = t;
                m_chromaticAbberrationIntensityMult = 1 - t;
                yield return new WaitForEndOfFrame();
            }
            m_filledBackground.alpha = 1;
        }

        StartCoroutine(FadeCoroutine());
    }
}
