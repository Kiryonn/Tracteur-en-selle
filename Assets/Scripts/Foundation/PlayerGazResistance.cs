using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGazResistance : MonoBehaviour
{
    [SerializeField] float gazResistance = 100f;
    [SerializeField] float currentGazValue = 0f;
    [SerializeField] float gazFillSpeed = 0.5f;
    public bool insideGaz;

    // Update is called once per frame
    void Update()
    {
        if (insideGaz)
        {
            currentGazValue += gazFillSpeed;
        }
        else
        {
            currentGazValue = currentGazValue <= 0 ? 0 : currentGazValue-gazFillSpeed;
        }
    }
}
