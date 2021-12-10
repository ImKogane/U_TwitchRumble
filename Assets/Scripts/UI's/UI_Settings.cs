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

    private void Start()
    {
        float musicVolumeValue = PlayerPrefs.GetFloat("musicVolume");
        musicVolumeSlider.SetValueWithoutNotify(musicVolumeValue);
        AudioManager.Instance.MusicVolumeChanged(musicVolumeValue);
        
        float sfxVolumeValue = PlayerPrefs.GetFloat("sfxVolume");
        musicVolumeSlider.SetValueWithoutNotify(sfxVolumeValue);
        AudioManager.Instance.MusicVolumeChanged(sfxVolumeValue);
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
