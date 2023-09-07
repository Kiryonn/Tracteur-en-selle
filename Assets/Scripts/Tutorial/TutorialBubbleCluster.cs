using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBubbleCluster : MonoBehaviour
{
    [SerializeField] List<TutorialBubble> clusterList;
    [SerializeField] GameObject clusterBackground;
    public bool triggered { get; private set; }
    private void Start()
    {
        
        //clusterList = new List<TutorialBubble>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            triggered = true;
            if (clusterBackground)
            {
                clusterBackground.SetActive(true);
            }
            StartCoroutine(StartTutorialSerie());
        }
    }

    IEnumerator StartTutorialSerie()
    {
        //GameManager.Instance.player.canMove = false;
        foreach (var cluster in clusterList)
        {
            StartCoroutine(cluster.LaunchTutorial());
            while (!cluster.triggered)
            {
                yield return null;
            }
            
        }
        if (clusterBackground)
        {
            clusterBackground.SetActive(false);
        }
        //GameManager.Instance.player.canMove = true;
        
    }
}
