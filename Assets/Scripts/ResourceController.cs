using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceController : MonoBehaviour
{
    PlayerUI playerUI;
    [SerializeField] float maxEnergy;
    [SerializeField] float energy;

    [SerializeField] float energyFillSpeed;
    [SerializeField] float speedMultiplier;
    DialogueVelo dialogueVelo;

    [SerializeField] float tempS;
    [SerializeField] float speedDecay;

    [System.NonSerialized] public UnityEvent<PlayerUI, float> energyChangeEvent;
    [System.NonSerialized] public UnityEvent<PlayerUI, float> speedChangeEvent;
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
        if (Input.GetKey(KeyCode.Space))
        {
            UpdateFakeSpeed();
        }
        tempS -= speedDecay * Time.deltaTime;
        if (tempS <= 0)
        {
            tempS = 0;
        }
        //energyFillSpeed = dialogueVelo.speed
        speedChangeEvent.Invoke(playerUI, tempS / 24f);
        FillEnergy();
    }
    void UpdateFakeSpeed()
    {
        tempS += speedMultiplier * Time.deltaTime;
        if (tempS >= 24)
        {
            tempS = 24;
        }
        
        
    }
    void FillEnergy()
    {
        if (tempS < 6)
        {
            energyFillSpeed = -1f;
        }else if (tempS < 18)
        {
            energyFillSpeed = 1f;
        }
        else
        {
            energyFillSpeed = 2f;
        }

        energy += energyFillSpeed * Time.deltaTime;
        if (energy > maxEnergy) energy = maxEnergy;
        if (energy < 0) energy = 0;

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

