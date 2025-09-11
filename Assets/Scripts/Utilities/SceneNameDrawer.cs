using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as SceneNameAttribute;
        string[] nameList = (attr != null) ? attr.NameList : Array.Empty<string>();

        // 兜底：没有选择项就用默认绘制
        if (nameList.Length == 0)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        if (property.propertyType == SerializedPropertyType.String)
        {
            int idx = Array.IndexOf(nameList, property.stringValue);
            idx = Mathf.Clamp(idx, 0, nameList.Length - 1);
            idx = EditorGUI.Popup(position, label.text, idx, nameList);
            property.stringValue = nameList[idx];
        }
        else if (property.propertyType == SerializedPropertyType.Integer)
        {
            int cur = Mathf.Clamp(property.intValue, 0, nameList.Length - 1);
            property.intValue = EditorGUI.Popup(position, label.text, cur, nameList);
        }
        else
        {
            EditorGUI.PropertyField(position, property, label); // 正确的回退
        }
    }

}

