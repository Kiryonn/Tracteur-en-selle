using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    [SerializeField] Volume pprocess;
    LensDistortion lensDistortion;
    // Start is called before the first frame update
    void Start()
    {
        if (pprocess.profile.TryGet(out lensDistortion))
        {
            Debug.Log("Lens Distortion effect detected");
        }
    }

    public void ChangeLensDistord(float to)
    {
        lensDistortion.intensity.SetValue(new ClampedFloatParameter(to, -1f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
