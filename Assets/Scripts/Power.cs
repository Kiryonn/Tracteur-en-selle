using System.Transactions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Power : MonoBehaviour
{
    
    public float TimeToDestroyObstacle;

    //Parameters
    private int _LocationNumber = 0;//if there is multiple spot
    private float _timeToDestroyObstacle;
    private Scrollbar JaugePower;
    private ColorBlock colorPower;

    //define global attributes
    void Start()
    {
        JaugePower = GetComponent<Scrollbar>();
        ResetObstacle();
    }

    void Update()
    {

        
        if (DialogueVelo.Instance.instantaneousPower > 350f)
        {
            DialogueVelo.Instance.instantaneousPower = 350f;
        }
        JaugePower.size = DialogueVelo.Instance.instantaneousPower / 350f;
        Debug.Log(JaugePower.size);

            if (JaugePower.size > 0.795f)
            {
                colorPower = JaugePower.colors;
                colorPower.normalColor = Color.red;
                JaugePower.colors = colorPower;
            }

            if (JaugePower.size > 0.495f && JaugePower.size < 0.795f)
            {
                colorPower = JaugePower.colors;
                colorPower.normalColor = Color.yellow;
                JaugePower.colors = colorPower;
                GameManager.Instance.IncreaseBattery();
            }
            /*
        if (Input.GetKey(KeyCode.Tab))
        {

            JaugePower.size += 0.005f;

            if (JaugePower.size > 0.795f)
            {
                colorPower = JaugePower.colors;
                colorPower.normalColor = Color.red;
                JaugePower.colors = colorPower;
            }

            if (JaugePower.size > 0.495f && JaugePower.size < 0.795f)
            {
                colorPower = JaugePower.colors;
                colorPower.normalColor = Color.yellow;
                JaugePower.colors = colorPower;
                GameManager.Instance.IncreaseBattery();
            }
        }*/

        JaugePower.size -= 0.04f * Time.deltaTime;

        if (JaugePower.size < 0.795f)
        {
            colorPower = JaugePower.colors;
            colorPower.normalColor = Color.yellow;
            JaugePower.colors = colorPower;
        }


        if (JaugePower.size < 0.495f)
        {
            colorPower = JaugePower.colors;
            colorPower.normalColor = Color.green;
            JaugePower.colors = colorPower;
        }


    }

    public Scrollbar GetJaugeObstacle()
    {
        return JaugePower;
    }

    


    //reset to starting value -> should disappear if not using Instantiate and/or using Destroy
    public void ResetObstacle()//called by Tile[resetTile]
    {

        JaugePower.value = 0;
        colorPower = JaugePower.colors;
        colorPower.normalColor = Color.green;
        JaugePower.colors = colorPower;
    }

    
    

}