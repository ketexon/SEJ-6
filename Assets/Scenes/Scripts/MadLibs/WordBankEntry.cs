using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class WordBankEntry : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    string m_value = "";

    [SerializeField]
    float m_reboundTime = 2.0f;


    [SerializeField]
    TMP_Text m_text;

    Vector3? m_dragStartPosition = null;
    Vector3? m_dragEndPosition = null;
    Vector2 m_dragOffset;
    bool m_dragging = false;

    float m_dragEndTime;

    CanvasGroup m_canvasGroup;

    WordBlank owner = null;

    public string Word => m_value;

    void Reset()
    {
        m_text = GetComponentInChildren<TMP_Text>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_dragging = true;
        m_canvasGroup.blocksRaycasts = false;

        if(owner != null)
        {
            Unattach();
            
        }
        if(m_dragStartPosition == null)
        {
            m_dragStartPosition = transform.position;
        }

        m_dragOffset = (Vector2)transform.position - eventData.position;
        transform.position = (Vector3)(eventData.position + m_dragOffset) + transform.position.z * Vector3.forward;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector3)(eventData.position + m_dragOffset) + transform.position.z * Vector3.forward;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_dragEndPosition = transform.position;
        m_dragEndTime = Time.time;

        m_canvasGroup.blocksRaycasts = true;
        m_dragging = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        WordBankEntry entry = eventData.pointerDrag.GetComponent<WordBankEntry>();
        if(entry == null)
        {
            return;
        }

        if(owner != null)
        {
            owner.AttachWord(entry);
        }
    }

    void Awake()
    {
        m_text.text = m_value;
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (owner == null && !m_dragging && m_dragEndPosition is Vector3 endPos && m_dragStartPosition is Vector3 startPos )
        {
            float percent = (Time.time - m_dragEndTime) / m_reboundTime;
            //Debug.Log(percent);
            if(percent > 1)
            {
                transform.position = startPos;
                m_dragEndPosition = null;
            }
            else
            {
                transform.position = Vector3.Lerp(endPos, startPos, ReboundEasing(percent));
            }
        }
    }

    float ReboundEasing(float x) => 1 - (1 - x) * (1 - x);

    public void AttachToBlank(WordBlank owner)
    {
        this.owner = owner;
        transform.position = owner.transform.position;// + Vector3.up * (owner.transform as RectTransform).rect.height;
    }

    public void Unattach()
    {
        if (owner)
        {
            owner.RemoveWord();
            owner = null;
            m_dragEndPosition = transform.position;
            m_dragEndTime = Time.time;
        }
    }
}
