using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider _ambienceVolumeSlider;
    
    float _defaultSFXVolume;
    float _defaultMusicVolume;
    float _defaultAmbienceVolume;
    

    private void Start()
    {
        _defaultAmbienceVolume = AudioManager.Instance._defaultAmbienceVolume;
        _defaultMusicVolume = AudioManager.Instance._defaultMusicVolume;
        _defaultSFXVolume = AudioManager.Instance._defaultSFXVolume;
        SetupAudioValues();
    }

    public void MusicVolumeChanged(float value)
    {
        AudioManager.Instance.MusicVolumeChanged(value);
    }

    public void SFXVolumeChanged(float value)
    {
        AudioManager.Instance.SFXVolumeChanged(value);
    }

    public void AmbienceVolumeChanged(float value)
    {
        AudioManager.Instance.AmbienceVolumeChanged(value);
    }

    void SetupAudioValues()
    {
        float musicVolumeValue;
        float sfxVolumeValue;
        float ambienceVolumeValue;
        
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicVolumeValue = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicVolumeValue = _defaultMusicVolume;
        }
        
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            sfxVolumeValue = PlayerPrefs.GetFloat("sfxVolume");
        }
        else
        {
            sfxVolumeValue = _defaultSFXVolume;
        }
        
        if (PlayerPrefs.HasKey("ambienceVolume"))
        {
            ambienceVolumeValue = PlayerPrefs.GetFloat("ambienceVolume");
        }
        else
        {
            ambienceVolumeValue = _defaultAmbienceVolume;
        }
        
        musicVolumeSlider.value = musicVolumeValue;
        sfxVolumeSlider.value = sfxVolumeValue;
        _ambienceVolumeSlider.value = ambienceVolumeValue;
    }
    
}
