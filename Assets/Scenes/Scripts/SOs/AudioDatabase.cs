using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDatabase", menuName = "Audio Database")]
public class AudioDatabase : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public string Name;
        public AudioClip Clip;
    }

    [SerializeField]
    public List<Entry> AudioFiles;

    public AudioClip Get(string name)
    {
        foreach(Entry entry in AudioFiles)
        {
            if (entry.Name == name) return entry.Clip;
        }
        return null;
    }
}
