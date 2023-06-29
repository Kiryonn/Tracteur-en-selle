using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueVelo : MonoBehaviour
{

    public FitnessEquipmentDisplay velo;
    public bool connected;
    public float instantaneousPower;
    [SerializeField] float SupposedResistance;
    public float speed;
    public float elapsedTime;
    public int heartRate;
    public int distanceTraveled;
    public int cadence;
    public float maxSpeed = 24f;
    bool started = false;

    public int veloPente = 0;
    // Update is called once per frame
    void Update()
    {
        if (velo)
        {
            lectureVelo();
        }
        else
        {
            try
            {
                velo = FindAnyObjectByType<FitnessEquipmentDisplay>();
            }
            catch (System.Exception)
            {
                Debug.Log("Searching for Fitness");
                throw;
            }
        }

        if (!started && velo && GameManager.Instance != null)
        {
            GameManager.Instance.SetPenteScaledWithDmg();
            started = true;
        }
    }


    void lectureVelo()
    {
        if (velo.connected)
        {
            connected = true;
            instantaneousPower = velo.instantaneousPower;
            speed = velo.speed;
            elapsedTime = velo.elapsedTime;
            heartRate = velo.heartRate;
            distanceTraveled = velo.distanceTraveled;
            cadence = velo.cadence;
            //velo.RequestCommandStatus();

        }
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
    }

    public void ChangeResitance(int n)
    {
        if (velo)
        {
            velo.SetTrainerResistance(n);
        }
        
    }

    public void ChangePente(int n)
    {
        if (velo)
        {
            velo.SetTrainerResistance(n);
            veloPente = n;
        }
        else
        {
            Debug.Log("There is no velo");
        }
        
        /*
        StopCoroutine("AsyncSetSlope");
        StartCoroutine(AsyncSetSlope(n, 6));
        */
        //Debug.Log("La pente est maintenant a : " + n);
    }

    IEnumerator AsyncSetSlope(int intensity, int loop)
    {
        for (int i = 0; i < loop; i++)
        {
            velo.SetTrainerSlope(intensity);
            Debug.Log("Setting slope to " + intensity);
            yield return new WaitForSeconds(1f);
        }
        veloPente = intensity;
    }
}
