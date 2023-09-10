using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class DialogueSceneManager : MonoBehaviour
{
    public static DialogueSceneManager Instance;

    [SerializeField]
    string m_homeScene = "Menu";

    [SerializeField]
    DialogueRunner m_dialogueRunner;

    [SerializeField]
    CanvasGroup m_blackScreen;

    [SerializeField]
    float m_fadeDuration = 1f;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }


    [YarnCommand("go_home")]
    public static void GoHome()
    {
        Instance.GoHomeImpl();
    }

    void GoHomeImpl()
    {
        GlobalState.PlayedOnce = true;

        if (m_dialogueRunner.VariableStorage.TryGetValue("$player_name", out string playerName))
        {
            GlobalState.PlayerName = playerName;
        }

        if (m_dialogueRunner.VariableStorage.TryGetValue("$time_period", out string timePeriod))
        {
            if (timePeriod.StartsWith("19"))
            {
                GlobalState.PathsSeen.Time1920s = true;
            }
            else if (timePeriod.StartsWith("20"))
            {
                GlobalState.PathsSeen.Time2060s = true;
            }
        }

        if (m_dialogueRunner.VariableStorage.TryGetValue("$friend_character", out string friend))
        {
            if (friend.ToLower().StartsWith("pirate"))
            {
                GlobalState.PathsSeen.Pirate = true;
            }
            else if(friend.ToLower().StartsWith("shark"))
            {
                GlobalState.PathsSeen.Shark = true;
            }
        }

        if (m_dialogueRunner.VariableStorage.TryGetValue("$clothing", out string clothing))
        {
            if (clothing.StartsWith("sock"))
            {
                GlobalState.PathsSeen.ClotheSock = true;
            }
            else if (clothing.StartsWith("wedding"))
            {
                GlobalState.PathsSeen.ClotheRing = true;
            }
        }

        if (m_dialogueRunner.VariableStorage.TryGetValue("$employer", out string employer))
        {
            if (employer.ToLower().StartsWith("info"))
            {
                GlobalState.PathsSeen.InfoBot = true;
            }
            else if (employer.ToLower().StartsWith("duck"))
            {
                GlobalState.PathsSeen.DuckGirl = true;
            }
        }

        var loadOp = SceneManager.LoadSceneAsync(m_homeScene);
        loadOp.allowSceneActivation = false;

        IEnumerator FadeOutCoroutine()
        {
            float startTime = Time.time;
            while (true)
            {
                float t = (Time.time - startTime) / m_fadeDuration;
                if (t > 1)
                {
                    m_blackScreen.alpha = 1;
                    break;
                }
                m_blackScreen.alpha = t;
                yield return new WaitForEndOfFrame();
            }
            loadOp.allowSceneActivation = true;
        }

        StartCoroutine(FadeOutCoroutine());
    }
}
