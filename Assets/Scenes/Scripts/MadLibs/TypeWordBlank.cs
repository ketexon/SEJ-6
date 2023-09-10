using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWordBlank : WordBlankBase
{
    [SerializeField]
    TMP_InputField m_inputField;

    protected override void Reset()
    {
        base.Reset();
        m_inputField = GetComponentInChildren<TMP_InputField>();
    }

    protected override void Awake()
    {
        base.Awake();
        m_inputField.onEndEdit.AddListener(OnEndEdit);
    }

    void OnEndEdit(string value)
    {
        Value = value.Length > 0 ? value : null;
    }
}