using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataScore
{
    public string ID;
    public float score;

    public DataScore(string ID, float score)
    {
        this.ID = ID;
        this.score = score;
    }
}
