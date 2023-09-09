using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordBlankBase : MonoBehaviour
{
    [SerializeField]
    public string VariableName;

    [SerializeField]
    string m_type;

    [SerializeField]
    TMP_Text m_label;

    private string value_;
    public string Value { 
        get => value_; 
        protected set {
            value_ = value;
            WordSetEvent?.Invoke(this);
        } 
    }

    public System.Action<WordBlankBase> WordSetEvent;

    virtual protected void Reset()
    {
        foreach (var text in GetComponentsInChildren<TMP_Text>())
        {
            if (text.gameObject.name.ToLower() == "label")
            {
                m_label = text;
            }
        }
    }

    virtual protected void Awake()
    {
        m_label.text = m_type;
    }
}
