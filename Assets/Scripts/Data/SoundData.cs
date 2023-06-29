using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Sound data",menuName ="Data/Sound Data")]
public class SoundData : ScriptableObject
{
    public AudioClip popClip;
    public AudioClip sucessClip;
    public AudioClip failClip;
    public AudioClip equipClip;
    public AudioClip recupClip;
}
