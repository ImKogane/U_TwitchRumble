using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private GameObject ConnectionCanvas;
    [SerializeField] private TwitchManager TwitchManager;
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject ConnectButton;

    // Start is called before the first frame update

    public void OpenLobby()
    {
        TwitchManager.Instance.canJoinedGame = true;
        SceneManager.LoadScene("ConnexionTwitch", LoadSceneMode.Single);
    }
    
    public void OpenSettings()
    {
        SettingsCanvas.SetActive(true);
    }
    
    public void OpenConnect()
    {
        ConnectionCanvas.SetActive(true);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void DisplayPlayButton()
    {
        PlayButton.SetActive(true);
        ConnectButton.SetActive(false);
    }


}

