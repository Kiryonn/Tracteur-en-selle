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
        image.color = pulse.Evaluate(0f);
    }


    private void Update()
    {
        /*
        image.color = pulse.Evaluate(Mathf.Sin(Time.time * pulseSpeed));

        rectTransform.localScale = Vector3.one * (1 + 0.5f * Mathf.Sin(Time.time));
        */
    }

    public void Pulse(float duration, float speed)
    {
        StartCoroutine(StartPulse(duration, speed));
    }

    IEnumerator StartPulse(float dur, float speed)
    {
        for (float i = 0f; i < 1.0f; i += Time.deltaTime / dur)
        {

            if (i > 0.5f)
            {
                image.color = pulse.Evaluate(1-i);
            }
            else
            {
                image.color = pulse.Evaluate(i);
            }
            rectTransform.localScale = Vector3.one * (i + 0.8f);
            yield return new WaitForEndOfFrame();
        }
    }

}
