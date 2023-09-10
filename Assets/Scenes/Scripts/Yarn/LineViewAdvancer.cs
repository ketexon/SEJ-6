using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

[RequireComponent(typeof(CustomLineView))]
public class DialogueAdvancer : MonoBehaviour, IPointerClickHandler
{
    CustomLineView lineView;

    void Awake() { 
        lineView = GetComponent<CustomLineView>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        lineView.UserRequestedViewAdvancement();
    }
}
