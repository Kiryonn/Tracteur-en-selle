using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningMaterial : Item
{
    public bool used { get; protected set; }
    public virtual void Use(NettoyageCuve net)
    {
        Debug.Log("Using " + name);
        used = true;
    }
}
