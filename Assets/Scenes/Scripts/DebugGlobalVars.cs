using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugGlobalVars : MonoBehaviour
{
    float? m_timeHeldStart;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9) && m_timeHeldStart == null)
        {
            m_timeHeldStart = Time.time;
        }
        else if(Input.GetKeyUp(KeyCode.F9) && m_timeHeldStart != null) {
            if(Time.time - m_timeHeldStart > 3.0f)
            {
                SetDebugVars();
                Debug.Log("DEBUG VARS SET");
            }
            m_timeHeldStart = null;
        }
    }

    void SetDebugVars()
    {
        GlobalState.PlayedOnce = true;
        GlobalState.PlayerName = "Aubrey";
        GlobalState.PathsSeen = new PathsSeen()
        {
            ClotheRing = true,
            ClotheSock = true,
            DuckGirl = true,
            Pirate = true,
            InfoBot = true,
            Shark = true,
            Time1920s = true,
            Time2060s = true,
            DrinkMilk = true,
            DrinkUnidentifiable = true,
        };
        SceneManager.LoadScene("Menu");
    }
}
