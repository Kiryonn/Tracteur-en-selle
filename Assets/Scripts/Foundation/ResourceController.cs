using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class ResourceController : MonoBehaviour
{
    PlayerUI playerUI;
    [SerializeField] float maxEnergy;
    [SerializeField] float energy;

    [SerializeField] float[] energyFillSpeed;
    [SerializeField] float speedMultiplier;
    DialogueVelo dialogueVelo;

    public float tempS { get; private set; }
    [SerializeField] float cadenceMax;
    [SerializeField] float cadenceFillSpeed;
    [SerializeField] float speedDecay;

    [System.NonSerialized] public UnityEvent<PlayerUI, float> energyChangeEvent;
    [System.NonSerialized] public UnityEvent<PlayerUI, float> speedChangeEvent;

    [SerializeField] Renderer batteryRenderer;
    [SerializeField] [ColorUsage(true, true)] Color lowBatteryColor;
    [SerializeField] [ColorUsage(true, true)] Color maxBatteryColor;
    Color batteryColor;
    Rigidbody rb;
    [SerializeField] float maxRbDrag;
    [SerializeField] float minRbDrag;

    [SerializeField] VisualEffect batteryR;
    [SerializeField] VisualEffect batteryL;

    [SerializeField] float maxBaterryDelay;
    [SerializeField] float maxBatteryCount;
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
        rb = GetComponent<Rigidbody>();
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

        HandleBatteryVFX();
    }

    void HandleBatteryVFX()
    {
        batteryL.SetFloat("Count", Mathf.Lerp(0f, maxBatteryCount, energy / maxEnergy));
        batteryR.SetFloat("Count", Mathf.Lerp(0f, maxBatteryCount, energy / maxEnergy));

        batteryL.SetFloat("Delay", Mathf.Lerp(6f, maxBaterryDelay, energy / maxEnergy));
        batteryR.SetFloat("Delay", Mathf.Lerp(6f, maxBaterryDelay, energy / maxEnergy));
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
        if (tempS > dialogueVelo.maxSpeed * 0.8f)
        {
            rb.drag = minRbDrag;
        }
        else
        {
            rb.drag = maxRbDrag;
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

