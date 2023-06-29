using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGazResistance : MonoBehaviour
{
    [SerializeField] float gazResistance = 100f;
    [SerializeField] float currentGazValue = 0f;
    [SerializeField] float gazFillSpeed = 0.5f;

    [Header("SFX")]
    [SerializeField] float maxBipDelay;
    [SerializeField] float minBipDelay;
    float currentBipDelay = 0f;
    [SerializeField] AudioClip bipClip;
    [SerializeField] AudioSource bipSource;

    public bool insideGaz;
    public bool dead;
    // Update is called once per frame
    void Update()
    {
        if (insideGaz && !dead)
        {
            currentGazValue += gazFillSpeed * Time.deltaTime;
            if (currentGazValue > gazResistance) { Die(); }
            currentBipDelay += Time.deltaTime;
            if (currentBipDelay > Mathf.Lerp(maxBipDelay,minBipDelay,currentGazValue/gazResistance))
            {
                bipSource.PlayOneShot(bipClip);
                currentBipDelay = 0f;
            }

        }
        else
        {
            currentGazValue = currentGazValue <= 0 ? 0 : currentGazValue-gazFillSpeed * Time.deltaTime;
        }
    }

    void Die()
    {
        if (!dead)
        {
            dead = true;
            currentGazValue = 0f;
        }
    }
}
