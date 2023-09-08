using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ToolsAnimation),true)]
public class ToolsAnimationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ToolsAnimation toolsAnim = (ToolsAnimation)target;

        if (GUILayout.Button("Hit"))
        {
            toolsAnim.TriggerAnimation();
        }
    }
}
