using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazZone : MonoBehaviour
{
    PlayerGazResistance playerGazResistance;
    [SerializeField] bool activated;
    [SerializeField] bool playerInside;

    private void Start()
    {
        playerGazResistance = GameManager.Instance.player.GetComponent<PlayerGazResistance>();
    }

    private void Update()
    {
        if (activated && playerInside)
        {
            playerGazResistance.insideGaz = true;
        }
        else
        {
            playerGazResistance.insideGaz = false;
        }
    }

    public void ActivateZone(float dur)
    {
        StartCoroutine(FreeGaz(dur));
    }

    IEnumerator FreeGaz(float duration)
    {
        activated = true;
        yield return new WaitForSeconds(duration);
        activated = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
