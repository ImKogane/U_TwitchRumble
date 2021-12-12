using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    public override bool DestroyOnLoad => false;
    
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AnimationCurve _audioCurve;

    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;
    

    public void MusicVolumeChanged(float value)
    {
        float dbValue = _audioCurve.Evaluate(value);
        _audioMixer.SetFloat("musicVolume", dbValue);
        PlayerPrefs.SetFloat("musicVolume", value);
    }

    public void SFXVolumeChanged(float value)
    {
        float dbValue = _audioCurve.Evaluate(value);
        _audioMixer.SetFloat("sfxVolume", dbValue);
        PlayerPrefs.SetFloat("sfxVolume", value);
    }

    public void PlayBGMusic(AudioClip newClip)
    {
        if (newClip != null)
        {
            _musicAudioSource.clip = newClip;
            _musicAudioSource.Play();
        }
        
    }
    
    
}
