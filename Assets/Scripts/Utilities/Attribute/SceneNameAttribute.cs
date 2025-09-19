using System.Collections.Generic;
using System.Text.RegularExpressions;
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
                text = System.IO.Path.GetFileNameWithoutExtension(val.path);
                if(Regex.IsMatch(text, @"^\d{2}\."))
                    list.Add(text);
            }
        }
        return list.ToArray();
    }

}
