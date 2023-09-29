using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RaptorStats",menuName ="Data/RaptorStats")]
public class RaptorStats : ScriptableObject
{
    public float detectionRange;

    public Vector2 sleepDurationRange;
    public float attackRange;
    public float maxIdleDuration;
    public float idleRange;

    public float chaseSpeed;
    public float normalSpeed;

    public float attackDmg;
}
