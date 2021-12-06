using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    public AudioSource audioSource;

    public override bool DestroyOnLoad => true;

    public void PlaySFX(AudioClip newAudioClip)
    {
        audioSource.PlayOneShot(newAudioClip);
    }

}
