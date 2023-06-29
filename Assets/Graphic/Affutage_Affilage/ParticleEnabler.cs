using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEnabler : StateMachineBehaviour
{
    ParticleSystem particle;
    AudioSource source;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        source = animator.GetComponent<AudioSource>();
        particle = animator.gameObject.GetComponentInChildren<ParticleSystem>();
        base.OnStateEnter(animator, stateInfo, layerIndex);
        source.Play();
        particle.Play();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        source.Stop();
        particle.Stop();
    }
}
