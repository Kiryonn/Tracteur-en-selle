using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Raptor : MonoBehaviour
{
    enum AIStates
    {
        Sleep,
        Idle,
        Search,
        Attacking,
        Flee,
        Chase
    }

    Animator anim;

    [SerializeField] Transform headPos;
    public NavMeshAgent agent { get; private set; }
    [Header("Locations")]

    public List<Transform> forests;

    [Header("Intelligence")]

    [SerializeField] RaptorStats raptorStats;

    [Header("Animation")]

    [SerializeField] Vector2 runSpeedRange;


    Vector3 lastKnownPosition;
    Transform playerPosition;
    Vector3 dir;

    AIStates currentState;
    AIStates previousState;
    AIStates nextState;

    RaycastHit hit;

    // Sleeps varibles

    float sleepTimer; // Gets incremented every Time.deltatime to count the sleep duration
    float sleepDuration; // Unlike the sleepDurationRange, it is a random value in the sleepDurationRange and is update each time the ai exit the sleep state

    //Idle variable

    float idleTimer; // Gets incremented every Time.deltatime to count the idle duration

    //Flee variable

    bool isFleeing; // Prevent the script from calling ChoseAWaypoint() in each update

    Vector3 agentDestination;

    bool attacked;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        hit = new RaycastHit();
        playerPosition = GameManager.Instance.velo.GetComponent<PlayerController>().transform;
        currentState = AIStates.Sleep;
        nextState = currentState;
        StartCoroutine(LOSChecks(0.2f));
    }

    public void InitAgentPosition(Vector3 pos)
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.Warp(pos);
        currentState = AIStates.Sleep;
        nextState = currentState;
    }

    private void Update()
    {
        switch (currentState)
        {
            case AIStates.Sleep:
                HandleSleep();
                break;
            case AIStates.Idle:
                HandleIdle();
                break;
            case AIStates.Search:
                HandleSearch();
                break;
            case AIStates.Attacking:
                HandleAttacking();
                break;
            case AIStates.Flee:
                HandleFlee();
                break;
            case AIStates.Chase:
                HandleChase();
                break;
            default:
                break;
        }

        if (currentState != AIStates.Chase)
        {
            anim.SetBool("Chase", false);
        }

        if (agent.velocity.magnitude >= 1f)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        HandleSpeed();

        UpdateAnimationsSpeeds();

        CacheState();
    }

    void CacheState()
    {
        if (currentState != nextState)
        {
            previousState = currentState;
            currentState = nextState;
        }
    }

    void HandleSpeed()
    {
        if (currentState != AIStates.Chase && currentState != AIStates.Flee)
        {
            agent.speed = raptorStats.normalSpeed;
        }
        else if (currentState == AIStates.Flee)
        {
            agent.speed = raptorStats.chaseSpeed * 2;
        }
        else
        {
            agent.speed = raptorStats.chaseSpeed;
        }

    }

    void HandleAttacking()
    {
        if (!attacked)
        {
            GameManager.Instance.player.GetComponent<DamageController>().DamageTractor(10f);
            //agent.isStopped = true;
            anim.SetTrigger("Roar");
            this.Invoke(() => {
                agent.isStopped = false;
                nextState = AIStates.Flee;
                attacked = false;
            }, 1.5f);
            attacked = true;
        }
        
    }

    void HandleFlee()
    {
        if (!isFleeing)
        {
            anim.SetBool("Chase", false);
            isFleeing = true;
            //agent.isStopped = false;
            agent.SetDestination(ChoseAWaypoint());

        }
        else if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            isFleeing = false;
            nextState = AIStates.Sleep;
        }
    }

    Vector3 ChoseAWaypoint()
    {
        int r = Random.Range(0, forests.Count);
        MyDebug.Log("int = "+r);

        return RandomNavSphere(forests[r].position, 1f, -1);
    }

    void HandleIdle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer > raptorStats.maxIdleDuration)
        {
            idleTimer = 0;
            
            SendTargetToRandomPosition();
        }
    }

    void HandleChase()
    {
        anim.SetBool("Chase", true);
        agent.SetDestination(lastKnownPosition);
        
        if (agent.remainingDistance <= raptorStats.attackRange && !agent.pathPending)
        {
            agent.isStopped = true;
            nextState = AIStates.Attacking;
        }
    }

    void HandleSleep()
    {
        sleepTimer += Time.deltaTime;
        if (sleepDuration == 0f)
        {
            sleepDuration = Random.Range(raptorStats.sleepDurationRange.x, raptorStats.sleepDurationRange.y);
        }
        if (sleepTimer > sleepDuration)
        {
            sleepTimer = 0f;

            nextState = AIStates.Idle;
            sleepDuration = Random.Range(raptorStats.sleepDurationRange.x, raptorStats.sleepDurationRange.y);
        }
    }

    void HandleSearch()
    {
        agent.SetDestination(lastKnownPosition);

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            nextState = AIStates.Sleep;
        }
    }

    void UpdateAnimationsSpeeds()
    {
        anim.SetFloat("RunSpeed", Mathf.Lerp(runSpeedRange.x, runSpeedRange.y, agent.velocity.magnitude / agent.speed));
    }

    void SendTargetToRandomPosition()
    {
        agent.SetDestination(RandomNavSphere(transform.position, raptorStats.idleRange, -1));
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    bool ExcludedStates()
    {
        return currentState != AIStates.Flee && currentState != AIStates.Attacking;
    }


    IEnumerator LOSChecks(float interval)
    {
        while (true)
        {
            Vector3 adjustedHeightedPosition = new Vector3(playerPosition.position.x, headPos.position.y, playerPosition.position.z);
            dir = (adjustedHeightedPosition - headPos.position).normalized;
            Debug.DrawRay(headPos.position, dir * raptorStats.detectionRange, Color.red, interval);

            if(Physics.Raycast(headPos.position, dir, out hit, raptorStats.detectionRange) && ExcludedStates())
            {
                if (hit.collider.gameObject.CompareTag("Player") )
                {
                    nextState = AIStates.Chase;
                    lastKnownPosition = hit.transform.position;
                }
                else if (currentState == AIStates.Chase)
                {
                    nextState = AIStates.Search;
                }
            }
            
            yield return new WaitForSeconds(interval);
        }
    } 
}
