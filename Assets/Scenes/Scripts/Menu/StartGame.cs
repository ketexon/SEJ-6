using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class StartGame : MonoBehaviour
{
    public static StartGame Instance;

    [SerializeField]
    string m_nextSceneName;

    [SerializeField]
    Button m_button;

    [SerializeField]
    MenuFader m_menuFader;

    [SerializeField]
    string m_secretScript = "Secret";

    [SerializeField]
    DialogueRunner m_dialogueRunner;

    [SerializeField]
    float m_fadeDuration = 1.0f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        m_button.onClick.AddListener(OnStartClicked);
    }

    [YarnCommand("disable_start")]
    public static void DisableStart()
    {
        Instance.DisableStartImpl();
    }

    void DisableStartImpl()
    {
        m_button.interactable = false;
    }

    [YarnCommand("enable_start")]
    public static void EnableStart()
    {
        Instance.EnableStartImpl();
    }

    void EnableStartImpl()
    {
        m_button.interactable = true;
    }

    void OnStartClicked()
    {
        if (GlobalState.PathsSeen.SeenAll)
        {
            m_dialogueRunner.StartDialogue(m_secretScript);
            return;
        }
        var loadOp = SceneManager.LoadSceneAsync(m_nextSceneName);
        loadOp.allowSceneActivation = false;

        m_menuFader.FadeOut(() =>
        {
            loadOp.allowSceneActivation = true;
        });
    }
}
