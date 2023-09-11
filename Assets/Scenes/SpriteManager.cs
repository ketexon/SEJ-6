using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instance;

    [SerializeField]
    ImageDatabase m_imageDatabase;

    [SerializeField]
    float m_transitionDuration = 1.0f;

    [SerializeField]
    RawImage m_backgroundImageFront;

    [SerializeField]
    RawImage m_backgroundImageBack;

    [SerializeField]
    RawImage m_foregroundImageFront;

    [SerializeField]
    RawImage m_foregroundImageBack;

    [SerializeField]
    List<StageSprite> m_sprites = new();

    Coroutine m_bgTransitionCoro = null;
    Coroutine m_fgTransitionCoro = null;

    Dictionary<string, StageSprite> m_spriteMap = new();

    void Reset()
    {
        m_sprites = new(GetComponentsInChildren<StageSprite>());
    }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"SpriteManager already has instance {Instance}. Destroying {this}...");
            Destroy(this);
        }
        Instance = this;

        foreach(var sprite in m_sprites)
        {
            if (m_spriteMap.ContainsKey(sprite.Name))
            {
                Debug.LogWarning($"Two sprites with the same name \"{sprite.Name}\"");
                continue;
            }
            m_spriteMap[sprite.Name] = sprite;
        }
    }

    void OnDestroy()
    {
        if (Instance != null)
        {
            Instance = null;
        }
    }

    [YarnCommand("set_bg")]
    public static void SetBackground(string name, bool instant = false)
    {
        Instance.SetBackgroundImpl(name, instant);
    }

    void SetBackgroundImpl(string name, bool instant)
    {
        Texture2D image = m_imageDatabase.Get(name);
        if(image == null)
        {
            Debug.LogError($"Tried to set background image to \"{name}\", which does not exist");
            return;
        }

        if (instant)
        {
            m_backgroundImageFront.texture = image;
            m_backgroundImageFront.color = Color.white;
            return;
        }

        IEnumerator TransitionCoroutine(System.Action callback)
        {
            m_backgroundImageBack.texture = m_backgroundImageFront.texture;
            if (m_backgroundImageBack.texture != null)
            {
                m_backgroundImageBack.color = Color.white;
            }

            m_backgroundImageFront.texture = image;
            m_backgroundImageFront.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            float startTime = Time.time;
            while (true)
            {
                float t = (Time.time - startTime) / m_transitionDuration;
                if (t > 1.0f)
                {
                    m_backgroundImageBack.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    m_backgroundImageFront.color = Color.white;
                    break;
                }

                if (m_backgroundImageBack.texture != null)
                {
                    m_backgroundImageBack.color = new Color(1.0f, 1.0f, 1.0f, 1 - t);
                }
                if (m_backgroundImageFront.texture != null)
                {
                    m_backgroundImageFront.color = new Color(1.0f, 1.0f, 1.0f, t);
                }
                yield return new WaitForEndOfFrame();
            }

            callback?.Invoke();
        }

        m_bgTransitionCoro = StartCoroutine(TransitionCoroutine(() =>
        {
            m_bgTransitionCoro = null;
        }));
    }

    [YarnCommand("set_fg")]
    public static void SetForeground(string name, bool instant = false)
    {
        Instance.SetForegroundImpl(name, instant);
    }

    void SetForegroundImpl(string name, bool instant)
    {
        Texture2D image = m_imageDatabase.Get(name);
        if (image == null)
        {
            Debug.LogError($"Tried to set foreground image to \"{name}\", which does not exist");
            return;
        }

        if (instant)
        {
            m_foregroundImageFront.texture = image;
            m_foregroundImageFront.color = Color.white;
            return;
        }

        IEnumerator TransitionCoroutine(System.Action callback)
        {
            m_foregroundImageBack.texture = m_foregroundImageFront.texture;
            if (m_foregroundImageBack.texture != null)
            {
                m_foregroundImageBack.color = Color.white;
            }

            m_foregroundImageFront.texture = image;
            m_foregroundImageFront.color = Color.clear;

            float startTime = Time.time;
            while (true)
            {
                float t = (Time.time - startTime)/m_transitionDuration;
                if(t > 1.0f)
                {
                    m_foregroundImageBack.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    m_foregroundImageFront.color = Color.white;
                    break;
                }

                if (m_foregroundImageBack.texture != null)
                {
                    m_foregroundImageBack.color = new Color(1.0f, 1.0f, 1.0f, 1 - t);
                }
                if(m_foregroundImageFront.texture != null)
                {
                    m_foregroundImageFront.color = new Color(1.0f, 1.0f, 1.0f, t);
                }
                yield return new WaitForEndOfFrame();
            }

            callback?.Invoke();
        }

        m_fgTransitionCoro = StartCoroutine(TransitionCoroutine(() =>
        {
            m_fgTransitionCoro = null;
        }));
    }

    [YarnCommand("set_sprite_variant")]
    public static void SetSpriteVariant(string name, string variant)
    {
        Instance.SetSpriteVariantImpl(name, variant);
    }

    void SetSpriteVariantImpl(string name, string variant)
    {
        if(m_spriteMap.ContainsKey(name))
        {
            m_spriteMap[name].SetVariant(variant);
        }
        else
        {
            Debug.LogError($"Sprite \"{name}\" does not exist");
            return;
        }
    }

    [YarnCommand("hide_sprite")]
    public static void HideSprite(string name)
    {
        Instance.HideSpriteImpl(name);
    }

    void HideSpriteImpl(string name)
    {
        if (m_spriteMap.ContainsKey(name))
        {
            m_spriteMap[name].Hide();
        }
    }

    [YarnCommand("show_sprite")]
    public static void ShowSprite(string name, string variant = null)
    {
        Instance.ShowSpriteImpl(name, variant);
    }

    void ShowSpriteImpl(string name, string variant)
    {
        if (m_spriteMap.ContainsKey(name))
        {
            if (variant != null && variant.Length > 0)
            {
                m_spriteMap[name].SetVariant(variant);
            }
            m_spriteMap[name].Show();
        }
    }
}
