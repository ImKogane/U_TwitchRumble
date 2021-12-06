using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_WinScreen : MonoBehaviour
{
    [SerializeField]
    private String MainMenuSceneName;
    [SerializeField]
    private String LobbySceneName
        ;
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

}