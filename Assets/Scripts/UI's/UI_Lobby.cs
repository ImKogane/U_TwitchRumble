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

    [SerializeField] private GameObject _loadGamePanel;

    private StartGameManager _startGameManager;
    
    void Start()
    {
        LobbyManager.Instance._uiLobby = this;
        PlayButton.SetActive(false);
        ErrorText.SetActive(true);
        _startGameManager = StartGameManager.Instance;
    }

    public void StartNewGame()
    {
        _startGameManager.SetLoadSavedGame(false);
        _startGameManager.LaunchGameLevel();
    }

    public void LoadSavedGame()
    {
        _startGameManager.SetLoadSavedGame(true);
        _startGameManager.LaunchGameLevel();
    }
    
    public void ShowLoadGameWindow(bool value)
    {
        _loadGamePanel.SetActive(value);
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
