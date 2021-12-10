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
    
    private void Start()
    {
        MusicVolumeChanged(PlayerPrefs.GetFloat("musicVolume"));
        SFXVolumeChanged(PlayerPrefs.GetFloat("sfxVolume"));
    }

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
    
    
}
