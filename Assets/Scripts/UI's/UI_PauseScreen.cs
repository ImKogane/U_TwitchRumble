using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PauseScreen : MonoBehaviour
{
    [Header("Sound settings")]
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _ambienceVolumeSlider;
    
    float _defaultSFXVolume;
    float _defaultMusicVolume;
    float _defaultAmbienceVolume;

    public GameObject _quitPanel;
    public TMP_Text _loadingText;
    
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
    
    public void ResumeGame()
    {
        GlobalManager.Instance.SetGamePause(false);
    }

    public void SetQuitPanelActive(bool value)
    {
        _quitPanel.SetActive(value);
    }

    public void SaveGame()
    {
        StartCoroutine(SaveCoroutine());
        
    }

    IEnumerator SaveCoroutine()
    {
        SaveSystem.SaveData();
        yield return null;
    }
    
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void GoToMenu()
    {
        GlobalManager.Instance.SetGamePause(false);
        PlayerManager.Instance.ResetPlayerManager();
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
        
        _musicVolumeSlider.value = musicVolumeValue;
        _sfxVolumeSlider.value = sfxVolumeValue;
        _ambienceVolumeSlider.value = ambienceVolumeValue;
    }
}
