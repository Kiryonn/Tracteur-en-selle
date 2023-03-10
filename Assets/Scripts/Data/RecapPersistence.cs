using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recap Persistence",menuName = "Data/Recap Persistence")]
public class RecapPersistence : ScriptableObject
{
    public Dictionary<BodyParts, float> medicalRecap;
}
