using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Propriétés des intéractions",menuName = "Data/Propriétés des intéractions")]  
public class InteractionProperties : ScriptableObject
{
    [ColorUsage(true,true)]
    public Color taskColor;
    [ColorUsage(true, true)]
    public Color itemColor;
    [ColorUsage(true, true)]
    public Color otherColor;
}
