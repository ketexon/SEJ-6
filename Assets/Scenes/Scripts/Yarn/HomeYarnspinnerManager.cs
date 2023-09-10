using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(DialogueRunner))]
public class HomeYarnspinnerManager : MonoBehaviour
{
    [SerializeField]
    string m_homeScript = "Home";

    DebugGlobalVars m_dbgGlobalVars;

    DialogueRunner m_dialogueRunner;

    bool m_playedHomeScene = false;

    void Awake()
    {
        m_dialogueRunner = GetComponent<DialogueRunner>();
        m_dbgGlobalVars = GetComponent<DebugGlobalVars>();
        if(m_dbgGlobalVars != null)
        {
            m_dbgGlobalVars.VarsSetEvent += PlayHomeScene;
        }
    }

    void Start()
    {
        m_dialogueRunner.SetInitialVariables();
        if (GlobalState.PlayedOnce)
        {
            PlayHomeScene();
        }
    }

    void PlayHomeScene()
    {
        if (m_playedHomeScene)
        {
            return;
        }
        m_playedHomeScene = true;

        m_dialogueRunner.VariableStorage.SetValue("$player_name", GlobalState.PlayerName);
        m_dialogueRunner.VariableStorage.SetValue("$seen_duck", GlobalState.PathsSeen.DuckGirl);
        m_dialogueRunner.VariableStorage.SetValue("$seen_pirate", GlobalState.PathsSeen.Pirate);
        m_dialogueRunner.VariableStorage.SetValue("$seen_shark", GlobalState.PathsSeen.Shark);
        m_dialogueRunner.VariableStorage.SetValue("$seen_ring", GlobalState.PathsSeen.ClotheRing);
        m_dialogueRunner.VariableStorage.SetValue("$seen_sock", GlobalState.PathsSeen.ClotheSock);
        m_dialogueRunner.VariableStorage.SetValue("$seen_infobot", GlobalState.PathsSeen.InfoBot);
        m_dialogueRunner.VariableStorage.SetValue("$seen_1920s", GlobalState.PathsSeen.Time1920s);
        m_dialogueRunner.VariableStorage.SetValue("$seen_2060s", GlobalState.PathsSeen.Time2060s);

        m_dialogueRunner.StartDialogue(m_homeScript);
    }
}
