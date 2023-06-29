using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TakeBottle), true)]
public class TakeBottleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        TakeBottle script = (TakeBottle)target;
        if (GUILayout.Button("Spawn bottle"))
        {
            script.SpawBottle();
        }
    }
}
