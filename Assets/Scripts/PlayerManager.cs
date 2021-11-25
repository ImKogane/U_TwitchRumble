using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public int PlayerToInstantiate = 1;

    public GameObject prefabOfPlayer;

    public List<string> AllPlayersName;

    [SerializeField]
    public List<Player> PlayerList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SpawnAllPlayerOnBoard()
    {
        for (int i = 0; i < PlayerToInstantiate; i++)
        {
            GameObject objinstantiate = Instantiate(prefabOfPlayer);

            Player player = objinstantiate.GetComponent<Player>();
            PlayerList.Add(player);

            Tile tileOfPlayer = BoardManager.Instance.GetRandomAvailableTile();

            player.SpawnPlayerInGame(tileOfPlayer);
        }
    }

    
}
