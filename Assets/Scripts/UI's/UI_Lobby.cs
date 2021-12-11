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
    
    // Start is called before the first frame update
    void Start()
    {
        PlayButton.SetActive(false);
        ErrorText.SetActive(true);  
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance.AllPlayersName.Count >= 2)
        {
            PlayButton.SetActive(true);
            ErrorText.SetActive(false);
        }
    }
    
    public void LaunchGame()
    {
        TwitchManager.Instance.canJoinedGame = false;
    }

    public void BackToMenu(string menuSceneName)
    {
        TwitchManager.Instance.canJoinedGame = false;
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
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
