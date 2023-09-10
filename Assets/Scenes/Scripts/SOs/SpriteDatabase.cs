using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteDB", menuName = "Sprite Database")]
public class SpriteDatabase : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        [SerializeField]
        public string Key;
        
        [SerializeField]
        public GameObject Value;
    };

    [SerializeField]
    public List<Entry> Entries;

    public GameObject Get(string key)
    {
        foreach(var e in Entries)
        {
            if(e.Key == key) return e.Value;
        }
        return null;
    }
}
