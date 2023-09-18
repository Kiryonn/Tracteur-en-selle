using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PulseUI),true)]
public class PulseUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PulseUI script = (PulseUI)target;

        if (GUILayout.Button("Pulse"))
        {
            script.Pulse(2f, 2f);
        }
    }
}
