using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using System;
using Yarn;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(MadLibs))]
public class MadLibsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button("Fetch all blanks"))
        {
            MadLibs madLibs = target as MadLibs;
            WordBlankBase[] blanks = madLibs.GetComponentsInChildren<WordBlankBase>();

            var blanksProperty = serializedObject.FindProperty("m_blanks");
            blanksProperty.ClearArray();
            for(int i = 0; i < blanks.Length; ++i)
            {
                blanksProperty.InsertArrayElementAtIndex(i);
                blanksProperty.GetArrayElementAtIndex(i).objectReferenceValue = blanks[i];
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif

[RequireComponent(typeof(CanvasGroup))]
public class MadLibs : MonoBehaviour
{
    [SerializeField]
    List<WordBlankBase> m_blanks = new();

    [SerializeField]
    Button m_submitButton;

    [SerializeField]
    float m_transitionDuration = 0.5f;

    [SerializeField]
    DialogueRunner m_dialogueRunner;

    [SerializeField]
    string m_nextYarnScript;

    CanvasGroup m_canvasGroup;

    Dictionary<string, string> m_blankValues = new();

    void Reset()
    {
        m_blanks = new(GetComponentsInChildren<WordBlankBase>());
        m_submitButton = GetComponentInChildren<Button>();
    }

    void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();

        m_submitButton.interactable = false;
        m_submitButton.onClick.AddListener(OnSubmit);

        foreach (var blank in m_blanks)
        {
            if (m_blankValues.ContainsKey(blank.VariableName))
            {
                Debug.LogError($"Blank {blank} has non-unique Variable Name {blank.VariableName}");
                continue;
            }
            if (blank.VariableName == null || blank.VariableName.Length < 2 || blank.VariableName[0] != '$')
            {
                Debug.LogError($"Blank {blank} has invalid Variable Name. It must be non-empty and start with $");
                continue;
            }
            m_blankValues[blank.VariableName] = null;
            blank.WordSetEvent += OnWordSet;
        }

        m_canvasGroup.interactable = false;
        StartCoroutine(EnterCoroutine());
    }

    public void Load(DialogueRunner dialogueRunner)
    {
        m_dialogueRunner = dialogueRunner;
    }

    void OnWordSet(WordBlankBase blank)
    {
        m_blankValues[blank.VariableName] = blank.Value;
        UpdateSubmitEnabled();
    }

    void UpdateSubmitEnabled()
    {
        bool disabled = false;
        foreach (var entry in m_blankValues)
        {
            if (entry.Value == null)
            {
                disabled = true;
                break;
            }
        }

        m_submitButton.interactable = !disabled;
    }

    void OnSubmit()
    {
        foreach(var entry in m_blankValues)
        {
            Debug.Log(m_dialogueRunner);
            m_dialogueRunner.VariableStorage.SetValue(entry.Key, entry.Value);
            Debug.Log($"VARIABLE STORAGE: {entry.Key} = \"{entry.Value}\"");
        }

        StartCoroutine(ExitCoroutine());
    }

    IEnumerator EnterCoroutine()
    {
        Vector3 startPos = new Vector3(
            transform.position.x,
            0,
            transform.position.z
        );
        Vector3 endPos = transform.position;
        return TransitionCoroutine(startPos, endPos, () =>
        {
            m_canvasGroup.interactable = true;
        });
    }

    IEnumerator ExitCoroutine()
    {
        m_canvasGroup.interactable = false;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(
            transform.position.x,
            0,
            transform.position.z
        );
        return TransitionCoroutine(startPos, endPos, () =>
        {
            m_dialogueRunner.StartDialogue(m_nextYarnScript);
            Destroy(gameObject);
        });
    }

    IEnumerator TransitionCoroutine(Vector3 startPos, Vector3 endPos, System.Action callback)
    {
        float startTime = Time.time;
        while (true)
        {
            float t = (Time.time - startTime) / m_transitionDuration;
            if (t >= 1)
            {
                transform.position = endPos;
                break;
            }
            transform.position = Vector3.Lerp(startPos, endPos, TransitionEasing(t));
            yield return new WaitForEndOfFrame();
        }
        callback?.Invoke();
    }

    float TransitionEasing(float x) => 1 - Mathf.Pow(1 - x, 3);
}
