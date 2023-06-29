using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomSliderGraphic : MonoBehaviour
{
    [SerializeField] GraphicSettings graphicSettings;
    [SerializeField] bool density;
    [SerializeField] bool distance;

    Slider slider;
    private void Start()
    {
        slider = GetComponent<Slider>();

        if (density)
        {
            slider.value = graphicSettings.grassDensity;
        }
        else
        {
            slider.value = graphicSettings.grassDistance/250f;
        }
    }

    public void ChangeValue(float f)
    {
        if (density)
        {
            graphicSettings.UpdateDensity(f);
        }
        else
        {
            graphicSettings.UpdateDistance(f * 250f);
        }
    }
}
