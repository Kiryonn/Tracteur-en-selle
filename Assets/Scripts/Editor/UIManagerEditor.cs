using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIManager))]
public class UIManagerEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIManager script = (UIManager)target;

        if (GUILayout.Button("Show Stylish Bar"))
        {
            script.ShowStylishBar();
        }
    }
}
