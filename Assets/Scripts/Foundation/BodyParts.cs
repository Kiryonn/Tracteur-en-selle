using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

[System.Serializable]
public class BodyParts
{
    public Parts partie;
    public VisualEffect painPoint;
}


public enum Parts
{
    Dos,
    Epaule,
    Coude,
    Main,
    Doigt,
    Poignet
}
