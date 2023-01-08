using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactable),true)]
public class Interactable_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        Interactable script = (Interactable)target;

        if (script.customColorization) // if bool is true, show other fields
        {

            script.customColor = EditorGUILayout.ColorField(script.customColor);
        }
    }
}
