using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BottleState
{
    Waiting,
    Processing,
    Ready,
    Available
}

public class Bottle : MonoBehaviour
{

    public float distanceTravelled;
    public float speed;
    public bool grabbed;

    float fillAmount;
    public float fillSpeed;
    
    public BottleState bottleState;
    public void UpdatePosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private void Start()
    {
        bottleState = BottleState.Waiting;
    }

    private void Update()
    {
        switch (bottleState)
        {
            case BottleState.Waiting:
                break;
            case BottleState.Processing:
                fillAmount += fillSpeed * Time.deltaTime;
                if (fillAmount > 1f)
                {
                    bottleState = BottleState.Ready;
                    fillAmount = 0f;
                }
                break;
            case BottleState.Ready:
                break;
            default:
                break;
        }
    }
}
