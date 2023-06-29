using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] Transform bobine;
    [SerializeField] float bobineSpeed;
    public float multiplier;

    // Update is called once per frame
    void Update()
    {
        bobine.Rotate(Vector3.up, bobineSpeed * Time.deltaTime * 10f * multiplier, Space.World);
    }
}
