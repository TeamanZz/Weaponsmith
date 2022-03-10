using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuietSFX : MonoBehaviour
{
    public static QuietSFX Instance;

    public List<AudioClip> soundsList = new List<AudioClip>();
    private AudioSource source;

    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlayAnvilHit()
    {
        source.PlayOneShot(soundsList[Random.Range(0, 3)]);
    }

    // public void PlayEnemyHit()
    // {
    //     source.PlayOneShot(soundsList[Random.Range(4, 6)]);
    // }

    // public void PlayEnemyDeath()
    // {
    //     source.PlayOneShot(soundsList[Random.Range(7, 8)]);
    // }
}