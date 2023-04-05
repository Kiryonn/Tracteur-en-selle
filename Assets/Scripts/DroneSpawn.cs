using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawn : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] float maxTimer;
    [SerializeField] float currentMaxTimer;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        currentMaxTimer = Random.Range(maxTimer, maxTimer*1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= currentMaxTimer)
        {
            anim.SetTrigger("Pass");
            currentMaxTimer = Random.Range(maxTimer, maxTimer * 1.5f);
            timer = 0f;
        }
    }
}
