using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    public override bool DestroyOnLoad => false;
    
    [Header("AudioMixer")]
    [SerializeField] private AudioMixer _audioMixer;
    
    [Header("Sounds Volume Curve")]
    [SerializeField] private AnimationCurve _audioCurve;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _ambianceAudioSource;

    [Header("Default Volume Values")]
    public float _defaultSFXVolume = .5f;
    public float _defaultMusicVolume = .5f;
    public float _defaultAmbienceVolume = .5f;
    
    
    #region ChangeVolumeValues
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

    public void AmbienceVolumeChanged(float value)
    {
        float dbValue = _audioCurve.Evaluate(value);
        _audioMixer.SetFloat("ambienceVolume", dbValue);
        PlayerPrefs.SetFloat("ambienceVolume", value);
    }
    #endregion
    
    public void PlayBGMusic(AudioClip newClip)
    {
        if (newClip != null)
        {
            _musicAudioSource.clip = newClip;
            _musicAudioSource.Play();
        }
        
    }

    public void EnableAmbienceSounds(bool value)
    {
        _ambianceAudioSource.gameObject.SetActive(value);
    }
}
