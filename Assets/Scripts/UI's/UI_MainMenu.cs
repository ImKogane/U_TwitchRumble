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
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private GameObject ConnectButton;

    // Start is called before the first frame update

    public void Start()
    {
        StartCoroutine(LateStart(1));
        
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (TwitchManager.Instance.IsConnected())
        {
            DisplayPlayButton();
        }
    }

    public void ButtonClick_Play()
    {
        TwitchManager.Instance.SetPlayersCanJoin(true);
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

