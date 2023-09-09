using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class WordBlank : WordBlankBase, IDropHandler
{
    WordBankEntry m_attachedWord = null;

    public void OnDrop(PointerEventData eventData)
    {
        WordBankEntry entry = eventData.pointerDrag.GetComponent<WordBankEntry>();
        if (entry == null)
        {
            Debug.LogWarning($"Received drop event from GameObject {eventData.pointerDrag}, which doesn't have a WordBankEntry component");
            return;
        }

        AttachWord(entry);
    }

    public void AttachWord(WordBankEntry entry)
    {
        if (m_attachedWord)
        {
            m_attachedWord.Unattach();
        }

        entry.AttachToBlank(this);
        m_attachedWord = entry;
        Value = entry.Word;
    }

    public void RemoveWord()
    {
        m_attachedWord = null;
        Value = null;
    }
}