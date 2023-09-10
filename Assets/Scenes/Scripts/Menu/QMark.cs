using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class QMark : MonoBehaviour
{
    public static bool Enable = false;

    [SerializeField]
    float m_rotateAngleMax = 10;

    [SerializeField]
    float m_rotateInterval = 3;

    [SerializeField]
    DebugGlobalVars m_dbgGlobalVars;

    float m_startTime = 0;

    void Awake()
    {
        m_startTime = Time.time;
        if (!GlobalState.PathsSeen.SeenAll)
        {
            GetComponent<Graphic>().color = Color.clear;
        }
        if(m_dbgGlobalVars != null)
        {
            m_dbgGlobalVars.VarsSetEvent += () =>
            {
                GetComponent<Graphic>().color = Color.white;
            };
        }
    }

    void Update()
    {
        float t = (Time.time - m_startTime)/m_rotateInterval;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(t * 2 * Mathf.PI) * m_rotateAngleMax);
    }
}
