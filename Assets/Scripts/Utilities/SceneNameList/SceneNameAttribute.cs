using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneNameAttribute : PropertyAttribute
{
    public string[] NameList => AllSceneNames();

    public static string[] AllSceneNames()
    {
        List<string> list = new List<string>();
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene val in scenes)
        {
            if (val.enabled)
            {
                string text = val.path.Substring(val.path.LastIndexOf('/') + 1);
                text = text.Substring(0, text.Length - 6);
                if(char.IsDigit(text[0]))
                    list.Add(text);
            }
        }
        return list.ToArray();
    }

}
