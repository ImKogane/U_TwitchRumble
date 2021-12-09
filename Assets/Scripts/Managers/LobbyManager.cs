using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyManager : SingletonMonobehaviour<LobbyManager>
{
    [Header("Reference UI Lobby")]
    public TextMeshProUGUI PlayerList;
    public GameObject CanvasLobby;
    public PlayerManager PlayerManager;

    public override bool DestroyOnLoad => false;

    void Start()
    {
        PlayerManager.Instance.AllPlayersName.Add(TwitchManager.Instance.channelName);
        TwitchManager.Instance.ShowAllPlayersInGame();
    }
    
    public void LaunchGame()
    {
        TwitchManager.Instance.canJoinedGame = false;
        ScenesManager.Instance.LaunchGameScenes();    
    }
}
