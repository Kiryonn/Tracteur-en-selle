using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntennaSignal : MonoBehaviour
{
    //[Header("Color values")]



    [SerializeField] float longDuration;
    [SerializeField] float shortDuration;
    [SerializeField] float delayDuration;

    [SerializeField] Renderer antennaRenderer;
    Material antennaColor;

    const byte k_MaxByteForOverexposedColor = 191;

    private void Start()
    {
        antennaColor = antennaRenderer.materials[1];
    }

    public void ReadMessage(string message)
    {
        StartCoroutine(ReadSignal(message));
    }

    public IEnumerator ReadSignal(string msg)
    {
        for (int i = 0; i < msg.Length; i++)
        {
            if (msg[i].Equals("-"))
            {
                antennaColor.color = ChangeHDRColorIntensity(antennaColor.color, 2.5f);
                yield return new WaitForSeconds(longDuration);
            }
            else
            {
                antennaColor.color = ChangeHDRColorIntensity(antennaColor.color, -10f);
                yield return new WaitForSeconds(shortDuration);
            }

            yield return new WaitForSeconds(delayDuration);
        }
    }

    Color ChangeHDRColorIntensity(Color subjectColor, float intensityValue)
    {
        // Get color intensity
        float maxColorComponent = subjectColor.maxColorComponent;
        float scaleFactorToGetIntensity = k_MaxByteForOverexposedColor / maxColorComponent;
        float currentIntensity = Mathf.Log(255f / scaleFactorToGetIntensity) / Mathf.Log(2f);

        // Get original color with zero intensity
        float currentScaleFactor = Mathf.Pow(2, currentIntensity);
        Color originalColorWithoutIntensity = subjectColor / currentScaleFactor;

        // Add Color intensity
        float modifiedIntensity = currentIntensity + intensityValue;

        // Set Color intensity
        float newScaleFactor = Mathf.Pow(2, modifiedIntensity);
        Color colorToReturn = originalColorWithoutIntensity * newScaleFactor;

        // Return color
        return colorToReturn;
    }
}
