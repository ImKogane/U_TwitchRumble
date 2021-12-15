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
        BoardManager.Instance.SetupBoard();
        ScenesManager.Instance.SetActiveScene("PlayersScene");
        PlayerManager.Instance.SetAllPlayerOnBoard();
        StartCoroutine(GlobalManager.Instance.LaunchNewGameCoroutine());
    }

    private async void LoadGame()
    {
        SaveData dataToLoad = await SaveSystem.LoadData();

        List<PlayerData> playerDatas = dataToLoad._playersDatas;
        List<TileData> tileDatas = dataToLoad._tilesDatas;
        
        foreach (TileData tileData in tileDatas)
        {
            BoardManager.Instance.LoadTile(tileData);
        }

        foreach (PlayerData playerData in playerDatas)
        {
            //PlayerManager.Instance.LoadPlayer(playerData)
        }

        GlobalManager.Instance.LoadSavedTurn(dataToLoad._currentTurn);

    }
    

}
