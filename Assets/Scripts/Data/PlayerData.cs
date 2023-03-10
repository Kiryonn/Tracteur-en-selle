using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Player Data",menuName ="Data/Player Data")]
public class PlayerData : ScriptableObject
{
    public string playerNickname;

    public float score;
}
