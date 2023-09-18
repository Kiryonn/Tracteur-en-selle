using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FillingMachine : MonoBehaviour
{
    [SerializeField] Transform pot;
    [SerializeField] float height;
    [SerializeField] TakeBottle creator;
    [SerializeField] GameObject flashParticle;
    public float workingSpeed;
    Bottle currentBottle;
    Vector3 offset;
    float dist;

    [SerializeField] bool readyingMachine;
    [SerializeField] AudioClip zapClip;

    [HideInInspector]
    public float multiplier = 1f;

    private void Start()
    {
        offset = pot.position;
    }
    public void StartProcess(Bottle bot)
    {
        bot.grabbed = false;
        LeanTween.moveY(bot.gameObject, bot.transform.position.y + height, 3f).setOnComplete(() =>
        {
            creator.AddBottle(bot);
            currentBottle = bot;
            bot.bottleState = BottleState.Processing;
            bot.fillSpeed = workingSpeed * multiplier;
            LeanTween.moveY(pot.gameObject, offset.y - height*0.2f, 1f);
        });
    }

    public void FinishProcess(Bottle bot)
    {
        bot.grabbed = true;
        bot.bottleState = BottleState.Waiting;
        
        LeanTween.moveY(bot.gameObject, bot.transform.position.y - height, 3f).setOnComplete(() =>
        {
            
            //LeanTween.moveY(pot.gameObject, offset.y - height*0.5f, 1f);
        });
    }

    private void Update()
    {
        if (currentBottle)
        {
            Vector3 targetPosition = currentBottle.transform.position;
            if (currentBottle.bottleState == BottleState.Waiting)
            {
                currentBottle.bottleState = BottleState.Processing;
            }
            
            currentBottle.fillSpeed = workingSpeed * multiplier;
            targetPosition.y = pot.position.y;

            pot.position = targetPosition;
            Vector3 closestPoint = creator.pathCreator.path.GetClosestPointOnPath(pot.position);
            dist = creator.pathCreator.path.GetClosestDistanceAlongPath(closestPoint);

            if (currentBottle.bottleState == BottleState.Ready)
            {
                if (readyingMachine)
                {
                    currentBottle.GetComponent<Renderer>().material.SetFloat("_Finition", 1f);
                    AudioManager.instance.PlaySFX(zapClip);
                    Destroy(Instantiate(flashParticle,currentBottle.transform),10f);
                }
                currentBottle = null;
                LeanTween.moveY(pot.gameObject, offset.y, 0.5f);
            }
        }
        else
        {
            //dist = creator.pathCreator.path.GetClosestDistanceAlongPath(closestPoint);
            dist -= creator.speed * 3 *Time.deltaTime;
            Vector3 targetPoint = creator.pathCreator.path.GetPointAtDistance(dist);
            if (dist > 0.1f)
            {
                targetPoint.y = pot.position.y;
                pot.position = targetPoint;
            }
            else
            {
                dist = 0;
            }
        }
    }
}
