using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEnabler : StateMachineBehaviour
{
    ParticleSystem particle;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        particle = animator.gameObject.GetComponentInChildren<ParticleSystem>();
        base.OnStateEnter(animator, stateInfo, layerIndex);
        particle.Play();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        particle.Stop();
    }
}
