using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Lobby : MonoBehaviour
{
    
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
        ScenesManager.Instance.LaunchGameScenes();    
    }

    public void BackToMenu(string menuSceneName)
    {
        TwitchManager.Instance.canJoinedGame = false;
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
    }
}
