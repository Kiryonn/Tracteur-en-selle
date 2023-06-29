using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PieCreator),true)]
public class PieCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PieCreator pieCreator = (PieCreator)target;

        if (GUILayout.Button("CreatePie"))
        {
            pieCreator.CreatePieFromComponent();
        }

    }
}
