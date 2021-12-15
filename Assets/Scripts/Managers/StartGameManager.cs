using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameManager : SingletonMonobehaviour<StartGameManager>
{
    public override bool DestroyOnLoad => false;

    private bool _loadingSavedGame = false;

    [SerializeField]
    private UI_Lobby _uiLobby;
    [SerializeField]
    private SO_level _gameLevel;
    
    private void Start()
    {
        if (SaveSystem.CheckSaveFile())
        {
            _uiLobby.ShowLoadGameWindow();
        }
        else
        {
            LobbyManager.Instance.SpawnLocalPlayer();
        }
    }

    public void SetLoadSavedGame(bool value)
    {
        _loadingSavedGame = value;
    }

    public void LaunchGame()
    {
        if (_loadingSavedGame)
        {
            LoadGame();
        }
        else
        {
            StartNewGame();
        }
    }
    
    public void EnableGameStart()
    {
        _uiLobby.EnablePlayButton();
    }

    public void LaunchGameLevel()
    {
        TwitchManager.Instance.canJoinedGame = false;
        _gameLevel.ChargeLevel();
    }
    
    private void StartNewGame()
    {
        ScenesManager.Instance.SetActiveScene("BoardScene");
        BoardManager.Instance.SetupNewBoard();
        ScenesManager.Instance.SetActiveScene("PlayersScene");
        PlayerManager.Instance.SetAllPlayerOnBoard();
        StartCoroutine(GlobalManager.Instance.LaunchNewGameCoroutine());
    }

    private async void LoadGame()
    {
        SaveData dataToLoad = await SaveSystem.LoadData();

        List<PlayerData> playerDatas = dataToLoad._playersDatas;
        List<TileData> tileDatas = dataToLoad._tilesDatas;

        
        ScenesManager.Instance.SetActiveScene("BoardScene");
        BoardManager.Instance.SetupCustomBoard();
        
        foreach (TileData tileData in tileDatas)
        {
            BoardManager.Instance.LoadTile(tileData);
        }

        ScenesManager.Instance.SetActiveScene("PlayersScene");
        foreach (PlayerData playerData in playerDatas)
        {
            PlayerManager.Instance.LoadPlayer(playerData);
            Debug.Log(playerData._playerName + " [" + playerData._playerTile + "]");
            Debug.Log(playerData._playerName + " [" + playerData._playerRotation + "]");
        }

        StartCoroutine(GlobalManager.Instance.LoadSavedTurn(dataToLoad._currentTurn));
    }
    

}
