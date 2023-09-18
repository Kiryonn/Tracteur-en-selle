using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CowCar : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform[] cowWheels;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < cowWheels.Length; i++)
        {
            cowWheels[i].Rotate(Vector3.right, speed);
        }
        
    }
}
