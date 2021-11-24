using UnityEngine;

public class Player : MonoBehaviour
{
    public Tile CurrentTile;

    public PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
       playerMovement.HandleMovement(Time.deltaTime);
    }

    public void SpawnPlayerInGame(Tile TileForStart)
    {
        CurrentTile = TileForStart;
        transform.position = CurrentTile.transform.position;
        TileForStart.hasPlayer = true;
    }
}
