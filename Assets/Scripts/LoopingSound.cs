using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Put this on the object you want to emit sound from
public class LoopingSound : MonoBehaviour
{
    AudioSource startSource;
    AudioSource loopSource;
    AudioSource endSource;

    [Header("Clips")]

    [SerializeField] AudioClip[] startClips;
    [SerializeField] AudioClip[] loopClips;
    [SerializeField] AudioClip[] endClips;

    double startTime;

    // Start is called before the first frame update
    void Awake()
    {
        startSource = gameObject.AddComponent<AudioSource>();
        loopSource = gameObject.AddComponent<AudioSource>();
        endSource = gameObject.AddComponent<AudioSource>();
    }

    AudioClip PickRandomClip(AudioClip[] clips)
    {
        int index = Random.Range(0, clips.Length);

        return clips[index];
    }

    public void PlayFromStart(double delay = 0d, bool loop = false)
    {
        startTime = AudioSettings.dspTime;

        startSource.clip = PickRandomClip(startClips);
        startSource.PlayScheduled(startTime + delay);

        double startDuration = startSource.clip.samples / startSource.clip.frequency;

        loopSource.clip = PickRandomClip(loopClips);
        loopSource.loop = true;
        loopSource.PlayScheduled(startTime + startDuration);

        double loopDuration = loopSource.clip.samples / loopSource.clip.frequency;

        if (!loop)
        {
            endSource.clip = PickRandomClip(endClips);

            endSource.PlayScheduled(startTime + loopDuration + startDuration);

            loopSource.loop = false;
        }
    }

    public void PlayLoop()
    {

    }
    public void Stop(bool fade = true)
    {
        endSource.clip = PickRandomClip(endClips);

        if (fade)
        {
            endSource.PlayScheduled(startTime + loopSource.clip.samples / loopSource.clip.frequency);

            loopSource.loop = false;
        }
        else
        {
            loopSource.Stop();

            endSource.Play();
        }
        startSource.Stop();
        
    }

    
}
