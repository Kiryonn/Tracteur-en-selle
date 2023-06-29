using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class NightTime : MonoBehaviour
{
    [SerializeField] Light lightSource;
    public bool dayNightCycle;
    SkyboxBlender skyboxBlender;
    [SerializeField] float timer;
    public float dayTimer;
    public float nightTimer;
    public float transitionSpeed;
    bool day;
    [SerializeField] GameObject[] spotLights;
    // Start is called before the first frame update
    void Start()
    {
        skyboxBlender = GetComponent<SkyboxBlender>();
        skyboxBlender.blend = 0f;
        day = true;
        //StartCoroutine("WaitForNight");
    }
    /*
    private void Update()
    {
        if (dayNightCycle)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
        }
        
        if (day && timer >= dayTimer*60f)
        {
            timer = 0;
            TurnSpotlights(true);
            StartCoroutine(ChangeDayTime(transitionSpeed, 0f));
            day = false;
        }

        if (!day && timer >= nightTimer*60f)
        {
            timer = 0;
            TurnSpotlights(false);
            StartCoroutine(ChangeDayTime(transitionSpeed, 1f));
            day = true;
        }

    }

    */
    // aValue = 1 -> transition vers le jour
    IEnumerator ChangeDayTime(float aTime, float aValue)
    {
        float currentIntensity = lightSource.intensity;
        float currentBlend = skyboxBlender.blend;
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / aTime)
        {
            skyboxBlender.blend = Mathf.Lerp(currentBlend, 1 - aValue, i);
            if (aValue == 1f)
            {
                lightSource.intensity = Mathf.Lerp(currentIntensity, 1f, i);
            }
            else
            {
                lightSource.intensity = Mathf.Lerp(currentIntensity, 0f, i);
            }
            
            yield return null;
        }
        lightSource.intensity = aValue;
        skyboxBlender.blend = 1-aValue;

        if (aValue == 1)
        {
            TurnSpotlights(false);
            day = true;
        }
        else
        {
            TurnSpotlights(true);
            day = false;
        }
        
    }

    public void FadeDayNight(float aTime, float aValue)
    {
        StartCoroutine(ChangeDayTime(aTime, aValue));
    }

    public void SetDayTime(bool yn)
    {
        if (yn)
        {
            StopAllCoroutines();
            TurnSpotlights(false);
            StartCoroutine(ChangeDayTime(3f, 1f));
            //lightSource.intensity = 1f;
            //skyboxBlender.blend = 0f;
            timer = 0f;
        }
    }

    void TurnSpotlights(bool on)
    {
        for (int i = 0; i < spotLights.Length; i++)
        {
            spotLights[i].GetComponent<DroneLight>().activated = on;
        }
    }
}
