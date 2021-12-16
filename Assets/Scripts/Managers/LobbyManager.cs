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
    public TextMeshProUGUI _playerList;
    public GameObject _canvasLobby;
    public PlayerManager _playerManager;

    public List<Tile> LobbyTileList = new List<Tile>();
    
    public UI_Lobby _uiLobby;

    private int _compteurBot = 1;

    public override bool DestroyOnLoad => true;

    void Start()
    {
        TwitchManager.Instance.numberMaxOfPlayer = GoogleSheetManager.Instance._variablesGetFromSheet[0];
        
        if (SaveSystem.CheckSaveFile())
        {
            _uiLobby.ShowLoadGameWindow(true);
        }
        else
        {
            _uiLobby.ShowLoadGameWindow(false);
            SpawnLocalPlayer();
        }
    }

    public void SpawnLocalPlayer()
    {
        string localPlayerName = TwitchManager.Instance.channelName;
        PlayerManager.Instance._listPlayersNames.Add(localPlayerName);
        PlayerManager.Instance.SpawnPlayerOnLobby(localPlayerName);
        TwitchManager.Instance.SetCanReadCommand(true);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerManager.Instance.SpawnPlayerOnLobby("[BOT" + _compteurBot + "]");
            _compteurBot++;
        }
    }

}
