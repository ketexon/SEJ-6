using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(VoiceOverLoader))]
class VoiceOverLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        YarnProject yarnProject = serializedObject.FindProperty("m_yarnProject").objectReferenceValue as YarnProject;
        if(yarnProject != null)
        {
            if(GUILayout.Button("Get line localizations"))
            {
                Localization localization = yarnProject.baseLocalization;
                foreach (var lineID in localization.GetLineIDs())
                {
                    string[] guids = FindVoiceOver.GetMatchingVoiceOverAudioClip(lineID, "en");
                    foreach (string guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        if(path != null && path.Length > 0)
                        {
                            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                            if(clip != null)
                            {
                                Debug.Log($"Successfully loaded audio clip {clip} for line \"{lineID}\"");
                                localization.SetLocalizedObject(lineID, clip);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}

#endif

public class VoiceOverLoader : MonoBehaviour
{
    [SerializeField]
    YarnProject m_yarnProject;
}
