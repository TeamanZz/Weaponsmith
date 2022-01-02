using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX Instance;

    public List<AudioClip> soundsList = new List<AudioClip>();
    private AudioSource source;

    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlayClick()
    {
        source.PlayOneShot(soundsList[0]);
    }

    public void PlayBuy()
    {
        source.PlayOneShot(soundsList[1]);
    }

    public void PlayMade()
    {
        source.PlayOneShot(soundsList[Random.Range(2, 4)]);
    }

    public void PlaySell()
    {
        source.PlayOneShot(soundsList[4]);
    }

    public void PlayEnvironmentSet()
    {
        source.PlayOneShot(soundsList[5]);
    }
}
