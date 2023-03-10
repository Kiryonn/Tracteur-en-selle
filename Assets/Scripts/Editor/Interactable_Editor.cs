using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Task),true)]
public class Interactable_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        Task script = (Task)target;
        serializedObject.Update();
        if (script.requireItem)
        {
            var list = serializedObject.FindProperty("requiredObjects");
            EditorGUILayout.PropertyField(list);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
