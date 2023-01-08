using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interactable : MonoBehaviour
{
    public string _name;
    public bool customColorization;
    [HideInInspector]
    [ColorUsage(true,true)]
    public Color customColor;

    public virtual void Interact()
    {
        Debug.Log("Interacting with " + _name);
    }
}
