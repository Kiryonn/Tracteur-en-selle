using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    PlayerController player;

    [Header("Drone related")]
    [SerializeField] Transform[] propellers;
    [SerializeField] float propellersSpeed;
    [SerializeField] float motorReactivity;
    [SerializeField] float chargeTime;
    Animator anim;
    float tempSpeed;
    bool charged;

    [Header("DeliveryBox")]
    [SerializeField] Transform grabPosition;
    [SerializeField] Transform box;
    [SerializeField] bool grabbed;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameManager.Instance.velo.GetComponent<PlayerController>();
        grabbed = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePropeller();
    }

    public void ToggleGrab()
    {
        if (grabbed)
        {
            box.SetParent(player.transform);
        }
        else
        {
            box.SetParent(transform);
            player.canMove = true;
            //SetTranform(box, grabPosition);
        }
        grabbed = !grabbed;
    }

    public void ToggleCharge()
    {
        if (!charged)
        {
            tempSpeed = propellersSpeed;
            StartCoroutine(TargetPropellerSpeed(0f, motorReactivity));
            StartCoroutine("ChargeDrone");
        }
        else
        {
            StartCoroutine(TargetPropellerSpeed(tempSpeed, motorReactivity));
            tempSpeed = 0f;
        }
        charged = !charged;
    }

    void HandlePropeller()
    {
        float r;
        for (int i = 0; i<propellers.Length; i++)
        {
            r = Random.Range(0.8f, 1.2f);
            propellers[i].Rotate(Vector3.forward, propellersSpeed * 10 * r * Time.deltaTime);
        }
    }

    IEnumerator ChargeDrone()
    {
        yield return new WaitForSeconds(chargeTime+motorReactivity);
        SendBackDrone();
    }

    IEnumerator TargetPropellerSpeed(float target, float duration)
    {
        float a = propellersSpeed;
        for(float i = 0f; i < 1f; i += Time.deltaTime / duration)
        {
            propellersSpeed = Mathf.Lerp(a, target, i);
            yield return null;
        }
        propellersSpeed = target;
    }

    void SendBackDrone()
    {
        anim.SetTrigger("Take");
    }

    public void SummonDrone()
    {
        anim.SetTrigger("Deliver");
        player.canMove = false;
    }

    void SetTranform(Transform from, Transform to)
    {
        from.position = to.position;
        from.rotation = to.rotation;
        from.localScale = to.localScale;
    }
}
