using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Lobby : MonoBehaviour
{
    [Header("UI canvas")]
    [SerializeField] private GameObject LobbyCanvas;
    [SerializeField] private GameObject HelpCanvas;

    [Header("UI elements")]
    [SerializeField] GameObject PlayButton;
    [SerializeField] GameObject ErrorText;
    
    void Start()
    {
        LobbyManager.Instance.uiLobby = this;
        
        PlayButton.SetActive(false);
        ErrorText.SetActive(true);
    }
    
    public void EnablePlayButton()
    {
        PlayButton.SetActive(true);
        ErrorText.SetActive(false);
    }

    public void ButtonClick_Menu()
    {
        PlayerManager.Instance.ResetPlayerManager();
    }
    
    public void LaunchGame()
    {
        TwitchManager.Instance.canJoinedGame = false;
    }
    
    public void ShowHelp(bool state)
    {
        if (state == true)
        {
            HelpCanvas.SetActive(true);
            LobbyCanvas.SetActive(false);
        }
        else
        {
            HelpCanvas.SetActive(false);
            LobbyCanvas.SetActive(true);
        }

    }
}
