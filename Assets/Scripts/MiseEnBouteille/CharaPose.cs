using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaPose : MonoBehaviour
{
    public void Pose()
    {
        GlobalMachine.instance.Spawn();
    }
}
