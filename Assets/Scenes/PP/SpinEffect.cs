using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Yarn.Unity;

[RequireComponent(typeof(PostProcessVolume))]
public class SpinEffect : MonoBehaviour
{
    public static SpinEffect Instance;

    [SerializeField]
    float m_spinInterval = 5;

    [SerializeField]
    float m_spinIntensityAmplitude = 0.3f;

    [SerializeField]
    float m_distortInterval = 3;

    [SerializeField]
    float m_distortIntensityAmplitude = 30f;


    PostProcessVolume m_volume;

    Spin m_spinSettings;
    LensDistortion m_distortSettings;

    bool m_spinEnabled;
    float m_spinStartTime;

    float m_intensityMult = 0.0f;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        m_volume = GetComponent<PostProcessVolume>();
        m_spinSettings = m_volume.profile.GetSetting<Spin>();
        m_distortSettings = m_volume.profile.GetSetting<LensDistortion>();

        m_spinSettings.intensity.value = 0;
        m_distortSettings.intensity.value = 0;
    }

    [YarnCommand("start_spin")]
    public static void StartSpin(string color1, string color2)
    {
        Instance.StartSpinImpl(color1, color2);
    }

    [YarnCommand("stop_spin")]
    public static void StopSpin()
    {
        Instance.StopSpinImpl();
    }

    Color StringToColor(string s)
    {
        string sn = s.ToLower();
        if (sn.StartsWith("red")) return Color.red;
        if (sn.StartsWith("blue")) return Color.blue;
        if (sn.StartsWith("green")) return Color.green;
        if (sn.StartsWith("purple")) return new Color(1, 0, 1);
        return Color.white;
    }

    void StartSpinImpl(string color1Str, string color2Str)
    {
        m_spinSettings.color1.value = StringToColor(color1Str); ;
        m_spinSettings.color2.value = StringToColor(color2Str); ;

        m_spinEnabled = true;
        m_spinStartTime = Time.time;
        IEnumerator EaseCoroutine()
        {
            float startTime = Time.time;
            while (true)
            {
                float t = (Time.time - startTime) / 2.0f;
                if (t > 1)
                {
                    m_intensityMult = 1.0f;
                    break;
                }
                m_intensityMult = t;
                yield return new WaitForEndOfFrame();
            }
        }
        StartCoroutine(EaseCoroutine());
    }

    void StopSpinImpl()
    {
        IEnumerator EaseCoroutine()
        {
            float startTime = Time.time;
            while (true)
            {
                float t = (Time.time - startTime) / 2.0f;
                if(t > 1)
                {
                    m_intensityMult = 0.0f;
                    break;
                }
                m_intensityMult = 1 - t;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            m_spinEnabled = false;
        }
        StartCoroutine(EaseCoroutine());
    }

    void Update()
    {
        if (m_spinEnabled)
        {
            float delta = Time.time - m_spinStartTime;

            m_spinSettings.intensity.value = Mathf.Sin(delta * Mathf.PI * 2 / m_spinInterval) * m_spinIntensityAmplitude * m_intensityMult;
            m_distortSettings.intensity.value = Mathf.Sin(delta * Mathf.PI * 2 / m_distortInterval)* m_distortIntensityAmplitude * m_intensityMult;
        }
    }
}
