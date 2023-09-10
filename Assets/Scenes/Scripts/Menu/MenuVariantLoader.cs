using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuVariantLoader : MonoBehaviour
{
    [SerializeField]
    CanvasGroup m_completeBackground;

    [SerializeField]
    CanvasGroup m_ringProp;

    [SerializeField]
    CanvasGroup m_sockProp;

    [SerializeField]
    CanvasGroup m_1920Prop;

    [SerializeField]
    CanvasGroup m_2060Prop;

    [SerializeField]
    CanvasGroup m_milkProp;

    [SerializeField]
    CanvasGroup m_liquidProp;

    void Start()
    {
        m_completeBackground.alpha = GlobalState.PathsSeen.SeenAll ? 1 : 0;
        if (!GlobalState.PathsSeen.SeenAll)
        {
            m_ringProp.alpha = GlobalState.PathsSeen.ClotheRing ? 1 : 0;
            m_sockProp.alpha = GlobalState.PathsSeen.ClotheSock ? 1 : 0;
            m_1920Prop.alpha = GlobalState.PathsSeen.Time1920s ? 1 : 0;
            m_2060Prop.alpha = GlobalState.PathsSeen.Time2060s ? 1 : 0;
            m_milkProp.alpha = GlobalState.PathsSeen.DrinkMilk ? 1 : 0;
            m_liquidProp.alpha = GlobalState.PathsSeen.DrinkUnidentifiable ? 1 : 0;
        }
    }
}
