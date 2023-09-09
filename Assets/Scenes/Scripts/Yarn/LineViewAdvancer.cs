using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

[RequireComponent(typeof(LineView))]
public class DialogueAdvancer : MonoBehaviour, IPointerClickHandler
{
    LineView lineView;

    void Awake() { 
        lineView = GetComponent<LineView>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        lineView.UserRequestedViewAdvancement();
    }
}
