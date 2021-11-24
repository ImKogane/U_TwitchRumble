using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private SO_PlayerStats PlayerStats;

    public Tile CurrentTile;

    public PlayerMovement playerMovement;

    public Quaternion InitialRotation;
    public Vector2Int RotationOfPlayer;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        InitialRotation = transform.rotation;
        RotateDownDirection();
    }

    void Update()
    {
       //playerMovement.HandleMovement(Time.deltaTime);
    }

    public void SpawnPlayerInGame(Tile TileForStart)
    {
        CurrentTile = TileForStart;
        transform.position = CurrentTile.transform.position;
        TileForStart.hasPlayer = true;
    }

    #region RotatePlayer
    public void RotateUpDirection()
    {
        RotationOfPlayer = new Vector2Int(0, 1);
        gameObject.transform.rotation = InitialRotation; 
        gameObject.transform.Rotate(0, 180, 0);
    }
    public void RotateDownDirection()
    {
        RotationOfPlayer = new Vector2Int(0, -1);
        gameObject.transform.rotation = InitialRotation;
    }
    public void RotateRightDirection()
    {
        RotationOfPlayer = new Vector2Int(1, 0);
        gameObject.transform.rotation = InitialRotation;
        gameObject.transform.Rotate(0, -90, 0);
    }
    public void RotateLeftDirection()
    {
        RotationOfPlayer = new Vector2Int(-1, 0);
        gameObject.transform.rotation = InitialRotation;
        gameObject.transform.Rotate(0, 90, 0);
    }
    #endregion

    public void MakeMovement()
    {
        if (CurrentTile)
        {
            Debug.Log("Current Tile : [" + CurrentTile.tileRow + "," + CurrentTile.tileColumn + "]");
        }
       
        Tile NextTile = null;

        //Avancer en X 
        if (RotationOfPlayer.x != 0)
        {
            Debug.Log("board manager : " + BoardManager.Instance);
            if (NextTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentTile.tileRow + RotationOfPlayer.x, CurrentTile.tileColumn)))
            {
                MoveToATile(NextTile);
            }
            else
            {
                FallInWater();
            }
        }
        //Avancer en Z
        else if (RotationOfPlayer.y != 0)
        {
            if (NextTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentTile.tileRow, CurrentTile.tileColumn + RotationOfPlayer.y)))
            {
                MoveToATile(NextTile);
            }
            else
            {
                FallInWater();
            }
        }
        else
        {
            Debug.Log("Le joueur est pas orienté en X ou en Z");
        }
    }

    public void MoveToATile(Tile nextTile)
    {
        Debug.Log("Cellule ciblée : [" + nextTile.tileRow + "," + nextTile.tileColumn + "]");

        if (nextTile.hasObstacle)
        {
            Debug.Log("Cellule ciblée est occupé par un obstacle.");
            return;
        }
        if (nextTile.hasPlayer)
        {
            Debug.Log("Cellule ciblée est occupé par un joueur.");
            return;
        }

        Debug.Log("Tile detecté, le mouvement peut etre fait !");
        CurrentTile.hasPlayer = false;
        CurrentTile = null;
        transform.position = nextTile.transform.position;
        CurrentTile = nextTile;
        CurrentTile.hasPlayer = true;
    }

    public void FallInWater()
    {
        Debug.Log("Aucune Tile detectée, vous plongez dans l'océan.");
    }
    
}
