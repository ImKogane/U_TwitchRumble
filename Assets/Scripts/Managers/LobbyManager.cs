using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LobbyManager : SingletonMonobehaviour<LobbyManager>
{
    [Header("Reference UI Lobby")]
    public TextMeshProUGUI PlayerList;
    public GameObject CanvasLobby;
    public PlayerManager PlayerManager;

    public List<Tile> LobbyTileList = new List<Tile>();

    public override bool DestroyOnLoad => false;

    void Start()
    {
        //Spawn the streamer as the local player
        string LocalPlayerName = TwitchManager.Instance.channelName;
        PlayerManager.Instance.AllPlayersName.Add(LocalPlayerName);
        PlayerManager.Instance.SpawnPlayerOnLobby(LocalPlayerName);
        
        TwitchManager.Instance.canJoinedGame = true;
    }
    
    
    
    public Tile GetRandomLobbyTile()
    {
        Tile tileToReturn = null;

        do{
            int indexOfTile = Random.Range(0, LobbyTileList.Count);
            tileToReturn = LobbyTileList[indexOfTile];
        }while (tileToReturn.hasPlayer == true);

        return tileToReturn;
    }
    
}
