using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTriggerScript : MonoBehaviour
{
    BigBagQuest quest;
    // Start is called before the first frame update
    void Start()
    {
        quest = (BigBagQuest)GameManager.Instance.currentQuest;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleTrigger()
    {
        quest.FillSemoir();
        Debug.Log("Particle entering collider");
    }
}
