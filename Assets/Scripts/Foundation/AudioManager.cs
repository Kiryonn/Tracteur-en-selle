using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] float fadeSpeed;
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioSource sfxAlt;
    public SoundData soundData;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1, bool alt = false, float limitedTime = 0f)
    {
        if (limitedTime == 0f)
        {
            if (alt)
            {
                sfxAlt.volume = volume;
                sfxAlt.PlayOneShot(clip);
            }
            else
            {
                sfx.volume = volume;
                sfx.PlayOneShot(clip);
            }
        }
        else
        {
            StartCoroutine(PlayerForSecond(limitedTime));
        }
        
        IEnumerator PlayerForSecond(float sec)
        {
            sfxAlt.clip = clip; 
            sfxAlt.volume = volume;

            sfxAlt.Play();

            yield return new WaitForSeconds(sec);

            sfxAlt.Stop();
        }
        
    }

    
    public void ScaryMusic()
    {
        music.pitch = -0.81f;
    }
    public void ChangeBackgroundMusic(AudioClip clip)
    {
        music.pitch = 1f;
        StartCoroutine(FadeMusic(fadeSpeed, clip, music));
    }

    IEnumerator FadeMusic(float duration,AudioClip clip,AudioSource source)
    {
        float baseVolume = source.volume;
        for (float i = 0.0f; i<1.0f; i += Time.deltaTime / duration)
        {
            source.volume = Mathf.Lerp(baseVolume, 0f, i);
            yield return null;
        }
        source.volume = 0f;
        source.clip = clip;
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / duration)
        {
            source.volume = Mathf.Lerp(0f, baseVolume, i);
            yield return null;
        }
        source.volume = baseVolume;
        source.Play();
        yield return null;
    }

    
}
