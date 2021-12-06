using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_WinScreen : MonoBehaviour
{
    [Header("Scenes names")]
    [SerializeField] private String MainMenuSceneName;
    [SerializeField] private String LobbySceneName;
        
    [Header("Element references")]
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject WinCamera;
    [SerializeField] private Text PlayerNameText;

    // Start is called before the first frame update

    public void PlayAgain()
    {
        TwitchManager.Instance.canJoinedGame = true;
        SceneManager.LoadScene(LobbySceneName, LoadSceneMode.Single);
    }
    
    public void BackToMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName, LoadSceneMode.Single);
    }

    public void SetPlayerNameText(String newText)
    {
        PlayerNameText.text = newText;
    }

    public void MainCameraEnabled(bool state)
    {
        MainCamera.SetActive(state);
    }
    
    public void WinCameraEnabled(bool state)
    {
        WinCamera.SetActive(state);
    }

}