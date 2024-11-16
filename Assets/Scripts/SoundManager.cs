using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource audioSource;

    //TODO: Add enum for all sounds to simplify use of PlaySound!

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Instance = this;
    }
    
    public void PlaySound(string sound)
    {
        audioSource.PlayOneShot(Resources.Load<AudioClip>(sound));
    }
}
