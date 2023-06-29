using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraumaFromColliding : MonoBehaviour
{
    CinemachineImpulseSource impulseSource;
    [SerializeField] float shakeCooldown;
    Camera cam;
    bool canShake = true;
    [SerializeField] float cameraDetectionSensivity = 0.57f;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        shakeCooldown = 3f;
        cameraDetectionSensivity = 0.57f;
    }

    IEnumerator Cooldown(float duration)
    {
        canShake = false;
        yield return new WaitForSeconds(duration);
        canShake = true;
    }

    bool CameraIsLooking()
    {
        Vector3 forwardVectorTowardsCamera = (cam.transform.position - transform.position).normalized;
        float dotProducResult = Vector3.Dot(cam.transform.forward, forwardVectorTowardsCamera);
        return dotProducResult < -cameraDetectionSensivity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CameraIsLooking() && canShake)
        {
            StartCoroutine(Cooldown(shakeCooldown));
            impulseSource.GenerateImpulse();
        }
        
    }
}
