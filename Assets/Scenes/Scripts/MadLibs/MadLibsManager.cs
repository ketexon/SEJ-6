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
    public static void RunLib(string libName)
    {
        Instance.RunLibImpl(libName);
    }

    void RunLibImpl(string libName)
    {
        var prefab = m_database.Get(libName);
        if (prefab == null)
        {
            Debug.LogError($"Tried to run lib \"{libName}\", but it does not exist.");
            return;
        }
        Instantiate(prefab, transform);
        prefab.GetComponent<MadLibs>().Load(m_dialogueRunner);
    }
}
