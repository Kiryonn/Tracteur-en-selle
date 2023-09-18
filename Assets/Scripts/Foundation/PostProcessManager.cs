using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    [SerializeField] Volume pprocess;
    LensDistortion lensDistortion;
    ColorAdjustments adjustments;
    // Start is called before the first frame update
    void Start()
    {
        if (pprocess.profile.TryGet(out lensDistortion))
        {
            MyDebug.Log("Lens Distortion effect detected");
        }

        if (pprocess.profile.TryGet(out adjustments))
        {
            MyDebug.Log("Color adjustement effect detected");
        }
    }

    public void ChangeLensDistord(float to)
    {
        lensDistortion.intensity.SetValue(new ClampedFloatParameter(to, -1f, 1f));
    }

    public void ChangeSaturation(float to)
    {
        adjustments.saturation.value = to;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ChangeSaturation(-100f);
            adjustments.contrast.value = 100f;
            adjustments.postExposure.value = -1f;
            AudioManager.instance.ScaryMusic();
            GameManager.Instance.player.GetComponent<DamageController>().DamageTractor(100f);

            gameObject.SetActive(false);
        }
    }
}
