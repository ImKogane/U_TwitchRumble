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

    [SerializeField] private float defaultSFXVolume = .5f;
    [SerializeField] private float defaultMusicVolume = .5f;

    private void Start()
    {
        float musicVolumeValue;
        float sfxVolumeValue;
        
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicVolumeValue = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicVolumeValue = defaultMusicVolume;
        }
        
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            sfxVolumeValue = PlayerPrefs.GetFloat("sfxVolume");
        }
        else
        {
            sfxVolumeValue = defaultSFXVolume;
        }
        
        musicVolumeSlider.value = musicVolumeValue;
        sfxVolumeSlider.value = sfxVolumeValue;
        
    }

    public void MusicVolumeChanged(float value)
    {
        AudioManager.Instance.MusicVolumeChanged(value);
    }

    public void SFXVolumeChanged(float value)
    {
        AudioManager.Instance.SFXVolumeChanged(value);
    }
    
}
