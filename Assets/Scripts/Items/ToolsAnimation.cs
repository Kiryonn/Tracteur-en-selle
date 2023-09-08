using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsAnimation : MonoBehaviour
{
    [SerializeField] AnimationCurve windupCurve; // Those curves need to go from 0 to 1
    [SerializeField] AnimationCurve hitCurve;

    [SerializeField] float shakeSpeed;
    [SerializeField] float shakeAmount;
    [SerializeField] float shakeDuration;

    [SerializeField] float windupAmount;
    [SerializeField] float swingAmount;

    [SerializeField] float windupSpeed;
    [SerializeField] float hitSpeed;

    [SerializeField] AudioClip hammerSound;


    public void TriggerAnimation()
    {
        StartCoroutine("HitAnim");
    }

    IEnumerator HitAnim()
    {
        Quaternion startQuat = transform.localRotation;
        float x = startQuat.eulerAngles.x;
        float y = startQuat.eulerAngles.y;
        float z = startQuat.eulerAngles.z;
        Debug.Log("Z is " + z);
        float newZ = 0f;
        for (float i = 0f; i < 1f; i += Time.deltaTime / windupSpeed)
        {
            newZ = Mathf.Lerp(z, z + windupAmount, windupCurve.Evaluate(i));

            transform.localRotation = Quaternion.Euler(x, y, newZ);
            yield return null;
        }

        float hitZ = 0f;
        for (float i = 0f; i < 1f; i += Time.deltaTime / hitSpeed)
        {
            hitZ = Mathf.Lerp(newZ, newZ - swingAmount, hitCurve.Evaluate(i));

            transform.localRotation = Quaternion.Euler(x, y, hitZ);
            yield return null;
        }

        Vector3 startPos = transform.localPosition;

        float rI;
        float rJ;
        float rK;

        AudioManager.instance.PlaySFX(hammerSound);

        for (float i = 0f; i < 1f; i += Time.deltaTime / shakeDuration)
        {
            rI = Random.Range(0.95f, 1.05f);
            rJ = Random.Range(0.95f, 1.05f);
            rK = Random.Range(0.95f, 1.05f);
            transform.localPosition = new Vector3(startPos.x + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount * rI,
                                                    startPos.y + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount * rJ,
                                                        startPos.z + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount * rK);
            
            yield return null;
        }

        transform.localPosition = startPos;
    }
}
