using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgroCow : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    enum AttackStates
    {
        Chaseing,
        Attack,
        Attacked
    }
    AttackStates attackStates = AttackStates.Chaseing;
    private void Start()
    {
        agent.Warp(agent.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        switch (attackStates)
        {
            case AttackStates.Chaseing:
                agent.SetDestination(GameManager.Instance.player.transform.position);
                break;
            case AttackStates.Attack:
                attackStates = AttackStates.Attacked;
                break;
            case AttackStates.Attacked:
                transform.position += transform.forward * Time.deltaTime * agent.speed;
                break;
            default:
                break;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackStates != AttackStates.Attacked)
        {
            attackStates = AttackStates.Attacked;
            agent.isStopped = true;
            GameManager.Instance.player.GetComponent<DamageController>().DamageTractor(30f);
            GameManager.Instance.GetComponent<TransitionManager>().FadeDamage(1f);
            Destroy(gameObject, 10f);
        }
    }
}
