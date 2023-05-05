using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceController : MonoBehaviour
{
    PlayerUI playerUI;
    [SerializeField] float maxEnergy;
    [SerializeField] float energy;

    [SerializeField] float[] energyFillSpeed;
    [SerializeField] float speedMultiplier;
    DialogueVelo dialogueVelo;

    [SerializeField] float tempS;
    [SerializeField] float cadenceMax;
    [SerializeField] float cadenceFillSpeed;
    [SerializeField] float speedDecay;

    [System.NonSerialized] public UnityEvent<PlayerUI, float> energyChangeEvent;
    [System.NonSerialized] public UnityEvent<PlayerUI, float> speedChangeEvent;

    [SerializeField] Renderer batteryRenderer;
    [SerializeField] [ColorUsage(true, true)] Color lowBatteryColor;
    [SerializeField] [ColorUsage(true, true)] Color maxBatteryColor;
    Color batteryColor;
    private void Start()
    {
        dialogueVelo = GetComponent<DialogueVelo>();
        tempS = 0;
        playerUI = GetComponent<PlayerUI>();
        if (energyChangeEvent == null)
        {
            energyChangeEvent = new UnityEvent<PlayerUI, float>();
        }

        if (speedChangeEvent == null)
        {
            speedChangeEvent = new UnityEvent<PlayerUI, float>();
        }

        UIManager.instance.SetListener(this);
    }

    private void Update()
    {
        UpdateFakeSpeed();
        if (Input.GetKey(KeyCode.Space))
        {
            tempS = dialogueVelo.maxSpeed;
        }
        /*tempS -= speedDecay * Time.deltaTime;
        if (tempS <= 0)
        {
            tempS = 0;
        }
        //energyFillSpeed = dialogueVelo.speed
        */
        speedChangeEvent.Invoke(playerUI, tempS / dialogueVelo.maxSpeed);
        FillEnergy();
    }
    void UpdateFakeSpeed()
    {
        if (tempS < dialogueVelo.speed * speedMultiplier)
        {
            tempS += cadenceFillSpeed * Time.deltaTime;
        }
        else
        {
            tempS -= cadenceFillSpeed * Time.deltaTime;
        } 
        //tempS += speedMultiplier * Time.deltaTime;
        if (tempS >= dialogueVelo.maxSpeed)
        {
            tempS = dialogueVelo.maxSpeed;
        }else if (tempS < 0)
        {
            tempS = 0;
        }
    }
    void FillEnergy()
    {
        float currentFill;
        if (tempS < dialogueVelo.maxSpeed / 3)
        {
            currentFill = energyFillSpeed[0];
        }else if (tempS < dialogueVelo.maxSpeed - (dialogueVelo.maxSpeed / 3))
        {
            currentFill = energyFillSpeed[1];
        }
        else
        {
            currentFill = energyFillSpeed[2];
        }

        energy += currentFill * Time.deltaTime;
        if (energy > maxEnergy) energy = maxEnergy;
        if (energy < 0) energy = 0;

        batteryColor = Color.Lerp(lowBatteryColor, maxBatteryColor, energy / maxEnergy);
        batteryRenderer.material.SetColor("_PowerColor", batteryColor);

        energyChangeEvent.Invoke(playerUI, energy/maxEnergy);
    }

    public void UseEnergy(float amount)
    {
        energy -= amount * Time.deltaTime;
    }

    public bool SuffisantEnergy()
    {
        return energy > 1;
    }
}

