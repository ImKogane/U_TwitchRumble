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

    public void LaunchGameLevel()
    {
        TwitchManager.Instance.canJoinedGame = false;
        _gameLevel.ChargeLevel();
    }
    
    private void StartNewGame()
    {
        BootstrapManager.Instance.SetActiveScene("BoardScene");
        BoardManager.Instance.SetupNewBoard();
        BootstrapManager.Instance.SetActiveScene("PlayersScene");
        PlayerManager.Instance.SetAllPlayerOnBoard();
        StartCoroutine(GlobalManager.Instance.LaunchNewGameCoroutine());
    }

    private async void LoadGame()
    {
        SaveData dataToLoad = await SaveSystem.LoadData();
        List<PlayerData> playerDatas = dataToLoad._playersDatas;
        List<TileData> tileDatas = dataToLoad._tilesDatas;
        
        BootstrapManager.Instance.SetActiveScene("BoardScene");
        BoardManager.Instance.SetupCustomBoard();
        
        foreach (TileData tileData in tileDatas)
        {
            BoardManager.Instance.LoadTile(tileData);
        }

        BootstrapManager.Instance.SetActiveScene("PlayersScene");
        foreach (PlayerData playerData in playerDatas)
        {
            PlayerManager.Instance.LoadPlayer(playerData);
        }

        StartCoroutine(GlobalManager.Instance.LoadSavedTurn(dataToLoad._currentTurn));
    }
    

}
