using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseUI : MonoBehaviour
{
    [SerializeField] Gradient pulse;

    [SerializeField] float pulseSpeed;
    Image image;
    RectTransform rectTransform;

    private void Start()
    {
        image = GetComponent<Image>();
        rectTransform = image.GetComponent<RectTransform>();
    }


    private void Update()
    {
        image.color = pulse.Evaluate(Mathf.Sin(Time.time * pulseSpeed));

        rectTransform.localScale = Vector3.one * (1 + 0.5f * Mathf.Sin(Time.time));
    }

}
