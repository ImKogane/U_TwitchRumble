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
            item.transform.position = tileOfPlayer.transform.position + (Vector3.up * 35);
            item._currentTile = tileOfPlayer;
            tileOfPlayer.currentPlayer = item;
            tileOfPlayer.hasPlayer = true;
        }

        //StartCoroutine(GlobalManager.Instance.LaunchNewGameCoroutine());
    }
    
    public void SpawnPlayerOnLobby(string playerName)
    {
        ScenesManager.Instance.SetActiveScene("PlayersScene");

        GameObject objinstantiate = Instantiate(prefabOfPlayer);

        Player player = objinstantiate.GetComponent<Player>();
        PlayerList.Add(player);

        if (PlayerList.Count >= 2)
        {
            StartGameManager.Instance.EnableGameStart();
        }
        
        Tile tileOfPlayer = LobbyManager.Instance.GetRandomLobbyTile();

        player.SpawnPlayerInGame(tileOfPlayer, playerName);
    }

    public Player ReturnPlayerWithName(string name)
    {
        foreach (Player player in PlayerList)
        {
            if (player._name == name)
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

    public List<int> GetAllPlayerHealth()
    {
        List<int> tempList = new List<int>();

        foreach (Player player in PlayerList)
        {
            tempList.Add(player._currentHealth);
        }

        return tempList;
    }

    public List<Vector2Int> GetAllPlayerTiles()
    {
        List<Vector2Int> tempList = new List<Vector2Int>();

        foreach (var player in PlayerList)
        {
            tempList.Add(new Vector2Int(player._currentTile.tileRow, player._currentTile.tileColumn));
        }

        return tempList;
    }

    public void ResetPlayerManager()
    {
        PlayerList.Clear();
        AllPlayersName.Clear();
    }

    public void LoadPlayer(PlayerData playerData)
    {
        Player newPlayer = Instantiate(prefabOfPlayer).GetComponent<Player>();
        newPlayer.InjectDatasFromSO();
        
        PlayerMovement newPlayerMovement = newPlayer.gameObject.GetComponent<PlayerMovement>();

        newPlayer._name = playerData._playerName;
        newPlayer._currentHealth = playerData._playerHealth;
        newPlayer.SetPlayerUI();

        newPlayerMovement.CurrentPlayer = newPlayer;
        Tile playerTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(playerData._playerTile.y, playerData._playerTile.x));
        newPlayerMovement.SetNewTile(playerTile);
        newPlayer.transform.position = newPlayer._currentTile.transform.position;

        newPlayerMovement.SetUpPlayerMovement(newPlayer);
        newPlayerMovement.RotatePlayerWithvector(playerData._playerRotation);

        newPlayer._skinnedMeshComponent.sharedMesh =
            SkinSystem.GetMeshAtIndex(playerData._skinnedMeshIndex);
        newPlayer._skinnedMeshComponent.material =
            SkinSystem.GetMaterialAtIndex(playerData._materialIndex);

        if (playerData._durationOfActiveFreezeDebuff.Count > 0)
        {
            foreach (var freezeDebuffDuration in playerData._durationOfActiveFreezeDebuff)
            {
                newPlayer._debuffList.Add(new FreezeDebuff(freezeDebuffDuration, newPlayer));
            }
        }

        if (playerData._durationOfActiveBurningDebuff.Count > 0)
        {
            foreach (var burningDebuffDuration in playerData._durationOfActiveBurningDebuff)
            {
                newPlayer._debuffList.Add(new BurningDebuff(burningDebuffDuration, newPlayer));
            }
        }

        if (playerData._playerChoices.Count > 0)
        {
            for (int i = 0; i < playerData._playerChoices.Count; i++)
            {
                SO_Choice newChoice = ScriptableManager.Instance.GetChoiceFromIndex(i, playerData._playerChoices[i]);
                newChoice.ApplyChoice(newPlayer);
            }
        }
        
        PlayerList.Add(newPlayer);
        
    }
    
}
