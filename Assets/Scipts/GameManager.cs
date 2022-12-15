using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public FitnessEquipmentDisplay velo;
    public bool connected;
    public int instantaneousPower;
    public float speed;
    public float elapsedTime;
    public int heartRate;
    public int distanceTraveled;
    public int cadence;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        connected = velo.connected;
        instantaneousPower = velo.instantaneousPower;
        speed=velo.speed;
        elapsedTime= velo.elapsedTime ;
        heartRate= velo.heartRate ;
        distanceTraveled= velo.distanceTraveled ;
        cadence= velo.cadence ;

           
        
    }
}
