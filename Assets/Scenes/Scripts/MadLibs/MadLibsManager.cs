using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Yarn.Unity;

public class MadLibsManager : MonoBehaviour
{
    public static MadLibsManager Instance { get; private set; }

    [SerializeField]
    MadLibsDatabase m_database;

    [SerializeField]
    AudioDatabase m_audioDatabase;

    [SerializeField]
    DialogueRunner m_dialogueRunner;

    void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning($"MadLibsManager already exists. Deleting {this}...");
            Destroy(this);
            return;
        }
        Instance = this;
    }

    [YarnCommand("run_lib")]
    public static void RunLib(string libName, string nextLine, string clipName = "")
    {
        Instance.RunLibImpl(libName, nextLine, clipName);
    }

    void RunLibImpl(string libName, string nextLine, string clipName)
    {
        var prefab = m_database.Get(libName);
        if (prefab == null)
        {
            Debug.LogError($"Tried to run lib \"{libName}\", but it does not exist.");
            return;
        }

        bool showPirateMap = false;
        if(m_dialogueRunner.VariableStorage.TryGetValue("$friend_character", out string friend) && friend.ToLower().StartsWith("pirate"))
        {
            showPirateMap = true;
        }

        AudioClip audioClip = m_audioDatabase.Get(clipName ?? "");
        var go = Instantiate(prefab, transform);
        go.GetComponent<MadLibs>().Load(m_dialogueRunner, nextLine, audioClip, showPirateMap);
    }
}
