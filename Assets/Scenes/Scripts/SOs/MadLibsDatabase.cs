using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MadLibsDB", menuName = "Mad Libs Database")]
public class MadLibsDatabase : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        [SerializeField]
        public string Name;

        [SerializeField]
        public GameObject Prefab;
    }

    [SerializeField]
    public List<Entry> Entries = new();

    public GameObject Get(string name)
    {
        foreach (var entry in Entries)
        {
            if (entry.Name == name) return entry.Prefab;
        }
        return null;
    }
}
