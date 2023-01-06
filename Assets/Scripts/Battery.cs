using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isAvailable =false;


    public bool IsAvailable
    {
        get
        {
            return isAvailable;
        }
        set
        {
            isAvailable = value;
            
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
