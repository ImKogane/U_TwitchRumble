using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : SingletonMonobehaviour<PlayerManager>
{
    public int PlayerToInstantiate = 1;

    public GameObject prefabOfPlayer;
    
    public SO_PlayerSkin SkinSystem;
    
    public List<string> AllPlayersName = new List<string>();
    
    public List<Player> PlayerList = new List<Player>();

    public override bool DestroyOnLoad => false;

    public void SetAllPlayerOnBoard()
    {
        foreach (Player item in PlayerList)
        {
            Tile tileOfPlayer = BoardManager.Instance.GetRandomAvailableTile();
            item.transform.position = tileOfPlayer.transform.position + (Vector3.up * 25);
        }
    }
    
    public void SpawnPlayerOnLobby(string playerName)
    {
        ScenesManager.Instance.SetActiveScene("PlayersScene");

        GameObject objinstantiate = Instantiate(prefabOfPlayer);

        Player player = objinstantiate.GetComponent<Player>();
        PlayerList.Add(player);

        Tile tileOfPlayer = LobbyManager.Instance.GetRandomLobbyTile();

        player.SpawnPlayerInGame(tileOfPlayer, playerName);
    }

    public Player ReturnPlayerWithName(string name)
    {
        foreach (Player player in PlayerList)
        {
            if (player.namePlayer == name)
            {
                return player;
            }
        }
        return null;
    }

    public void ManagePlayersDebuffs()
    {
        foreach (var player in PlayerList)
        {
            player.ManageAllDebuffs();
        }
    }
    
    public int GetPlayerCount()
    {
        return PlayerList.Count;
    }

    public Player GetLastPlayer()
    {
        if (GetPlayerCount() == 1)
        {
            return PlayerList[0];
        }
        else
        {
            return null;
        }
    }
    
    
}
