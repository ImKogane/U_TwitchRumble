using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public int PlayerToInstantiate = 1;

    public GameObject prefabOfPlayer;

    [NonSerialized]
    public List<string> AllPlayersName = new List<string>();

    [NonSerialized]
    public List<Player> PlayerList = new List<Player>();

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

    public void PlayGame()
    {
        TwitchManager.Instance.canJoinedGame = false;
        SceneManager.LoadScene("Antonin_Scene");
    }

    public void SpawnAllPlayerOnBoard()
    {
        foreach (string item in AllPlayersName)
        {
            GameObject objinstantiate = Instantiate(prefabOfPlayer);

            Player player = objinstantiate.GetComponent<Player>();
            PlayerList.Add(player);

            Tile tileOfPlayer = BoardManager.Instance.GetRandomAvailableTile();

            player.SpawnPlayerInGame(tileOfPlayer, item);
        }
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
    
    
}
