using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private TwitchManager TwitchManager;
    [SerializeField] private GameObject PlayButton;
    
    
    // Start is called before the first frame update

    public void OpenLobby()
    {
        SceneManager.LoadScene("ConnexionTwitch", LoadSceneMode.Single);
    }
    
    public void OpenSettings()
    {
        SettingsCanvas.SetActive(true);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Update()
    {
        if (TwitchManager.GetConnexionIsDone())
        {
            PlayButton.SetActive(true);
        }
        else
        {
            PlayButton.SetActive(false);
        }
    }
}
