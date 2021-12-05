using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;
    
    [Header("Reference UI Lobby")]
    public TextMeshProUGUI PlayerList;
    public GameObject CanvasLobby;
    public PlayerManager PlayerManager;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public void LaunchGame()
    {
        TwitchManager.Instance.canJoinedGame = false;
        ScenesManager.Instance.LaunchGameScenes();    
    }
}
