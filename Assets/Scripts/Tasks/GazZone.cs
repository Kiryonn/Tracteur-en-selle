using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazZone : MonoBehaviour
{
    PlayerGazResistance playerGazResistance;
    [SerializeField] bool activated;
    [SerializeField] bool playerInside;
    [SerializeField] float deadTime = 300f;
    [SerializeField] float currentDeadTime = 0f;
    [SerializeField] float activeTime = 20f;
    [SerializeField] Offset spawnPoint;
    [HideInInspector]
    public CO2UI co2UI;
    [SerializeField] CO2Detector detector;
    bool dying;
    private void Start()
    {
        playerGazResistance = GameManager.Instance.player.GetComponent<PlayerGazResistance>();
    }

    private void Update()
    {
        playerGazResistance.insideGaz = activated && playerInside;
        if (co2UI)
        {
            co2UI.alarming = activated && playerInside;
        }

        if (playerGazResistance.dead)
        {
            if (!dying)
            {
                GameManager.Instance.player.canMove = false;
                GameManager.Instance.player.playerAnim.SetBool("Die",true);
                dying = true;
            }
            
            currentDeadTime += Time.deltaTime;
            GameManager.Instance.player.canMove = false;
            if (currentDeadTime > deadTime)
            {
                playerGazResistance.dead = false;
                currentDeadTime = 0f;
                dying = false;
                GameManager.Instance.player.playerAnim.SetBool("Die", false);
                spawnPoint.SetOffset(GameManager.Instance.player.transform, Space.Self, true);
                Invoke("ReleasePlayer", 3f);
            }
        }
    }

    void ReleasePlayer()
    {
        GameManager.Instance.player.canMove = true;
        playerGazResistance.dead = false;
        GameManager.Instance.player.playerAnim.SetBool("Die", false);
    }

    public void ActivateZone()
    {
        StartCoroutine(FreeGaz(activeTime));
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
            if (co2UI) { StartCoroutine(co2UI.Alarm()); }
            playerGazResistance.gotDetector = GameManager.Instance.collectedItems.Contains(detector);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerGazResistance.gotDetector = GameManager.Instance.collectedItems.Contains(detector);
        }
    }
}
