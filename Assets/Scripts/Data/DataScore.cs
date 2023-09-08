using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataScore
{
    public string ID;
    public float score;
    public float time;
    public DataScore(string ID, float score, float time)
    {
        this.ID = ID;
        this.score = score;
        this.time = time;
    }
}
