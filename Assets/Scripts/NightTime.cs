using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class NightTime : MonoBehaviour
{
    [SerializeField] Light lightSource;
    SkyboxBlender skyboxBlender;
    public float dayTimer;
    public float nightTimer;
    public float transitionSpeed;
    [SerializeField] GameObject[] spotLights;
    // Start is called before the first frame update
    void Start()
    {
        skyboxBlender = GetComponent<SkyboxBlender>();
        skyboxBlender.blend = 0f;
        StartCoroutine("WaitForNight");
    }

    public IEnumerator WaitForNight()
    {
        yield return new WaitForSeconds(dayTimer * 60f);
        TurnSpotlights(true);
        StartCoroutine(ChangeDayTime(transitionSpeed, 0f));
        
    }

    public IEnumerator WaitForDay()
    {
        yield return new WaitForSeconds(nightTimer * 60f);
        TurnSpotlights(false);
        StartCoroutine(ChangeDayTime(transitionSpeed, 1f));
        
    }


    // aValue = 1 -> transition vers le jour
    IEnumerator ChangeDayTime(float aTime, float aValue)
    {

        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / aTime)
        {
            skyboxBlender.blend = Mathf.Abs(aValue-i);
            if (aValue == 1f)
            {
                lightSource.intensity = i;
            }
            else
            {
                lightSource.intensity = 1-i;
            }
            
            yield return null;
        }
        lightSource.intensity = aValue;
        skyboxBlender.blend = 1-aValue;

        if (aValue == 1f)
        {
            StartCoroutine("WaitForNight");
        }
        else
        {
            StartCoroutine("WaitForDay");
        }
    }

    public void SetDayTime(bool yn)
    {
        if (yn)
        {
            StopAllCoroutines();
            TurnSpotlights(false);
            lightSource.intensity = 1f;
            skyboxBlender.blend = 0f;
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
