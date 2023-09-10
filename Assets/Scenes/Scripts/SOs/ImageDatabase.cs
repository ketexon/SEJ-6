using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImageDB", menuName = "Image Database")]
public class ImageDatabase : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        [SerializeField]
        public string Key;

        [SerializeField]
        public Texture2D Value;
    };

    [SerializeField]
    public List<Entry> Entries;

    public Texture2D Get(string key)
    {
        foreach(var e in Entries)
        {
            if(e.Key == key) return e.Value;
        }
        return null;
    }
}
