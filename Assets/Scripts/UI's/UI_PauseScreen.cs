using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PauseScreen : MonoBehaviour
{
    [Header("Sound settings")]
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
    
    public void ResumeGame()
    {
        GlobalManager.Instance.SetGamePause(false);
    }

    public void QuitGame()
    {
        //Ask if player want to save the game
        
        Application.Quit();
    }

    public void GoToMenu()
    {
        GlobalManager.Instance.SetGamePause(false);
    }
}
