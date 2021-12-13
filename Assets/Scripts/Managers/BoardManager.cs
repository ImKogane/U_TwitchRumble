using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Configuration;
using UnityEngine;

public class BoardManager : SingletonMonobehaviour<BoardManager>
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject obstaclePrefab;

    private GameObject gameBoard;
    
    [HideInInspector]
    public List<Tile> tilesList = new List<Tile>();
    
    private float tileWidth;
    private float tileLength;
    
    [SerializeField] private int testBoardSizeX;
    [SerializeField] private int testBoardSizeZ;
    [SerializeField] private int testHoleNumber;
    [SerializeField] private int testObstaclesNumber;

    public override bool DestroyOnLoad => true;

    void Start()
    {
        //Set up variable with google sheet datas.
        testHoleNumber = GoogleSheetManager.Instance.VariablesGetFromSheet[1];
        testObstaclesNumber = GoogleSheetManager.Instance.VariablesGetFromSheet[2];

        MeshRenderer tileRenderer = tilePrefab.GetComponentInChildren<MeshRenderer>();
        
        if (tileRenderer)
        {
            tileLength = tileRenderer.bounds.size.z;
            tileWidth = tileRenderer.bounds.size.x;
        }
        
        CreateBoard(testBoardSizeX, testBoardSizeZ, testHoleNumber,testObstaclesNumber);
    }

    public void CreateBoard(int sizeX, int sizeZ, int holeNumber, int obstacleNumber)
    {
        #region Basic Board Creation

        ScenesManager.Instance.SetActiveScene("BoardScene");

        gameBoard = new GameObject("GameBoard");

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                float tempPosZ = j * tileLength;
                float tempPosX = i * tileWidth;

                Tile newTile = Instantiate(tilePrefab, new Vector3(tempPosX, 0, tempPosZ), Quaternion.identity, gameBoard.transform).GetComponent<Tile>();

                if (newTile)
                {
                    tilesList.Add(newTile);
                    newTile.SetCoord(new Vector2Int(i, j));
                }
                
            }
        }
        
        #endregion

        #region Holes placement

        List<Tile> tilesWithHole = GetRandomTiles(testHoleNumber);

        foreach (var tile in tilesWithHole)
        {
            tilesList.Remove(tile);
            Destroy(tile.gameObject);
        }

        #endregion
        
        #region Obstacle Placement
        
        List<Tile> tilesWithObstacle = GetRandomTiles(testObstaclesNumber);

        foreach (var tile in tilesWithObstacle)
        {
            tile.SetHasObstacle(true);
            Instantiate(obstaclePrefab, tile.transform.position, Quaternion.identity, gameBoard.transform);
        }

        #endregion

        ScenesManager.Instance.SetActiveScene("PlayersScene");

        
        
        PlayerManager.Instance.SetAllPlayerOnBoard();
        Debug.Log("Player Management : " + PlayerManager.Instance);
    }

    private List<Tile> GetRandomTiles(int tilesAmount)
    {
        int tempAmount = 0;

        List<Tile> tempTileList = new List<Tile>();
        
        while (tempAmount < tilesAmount)
        {
           
            int tempTileIndex;
            Tile tempTile;

            do
            {
                tempTileIndex = Random.Range(0, tilesList.Count);
                tempTile = tilesList[tempTileIndex];
                
                if(CheckPlacementAvailable(tempTile) && tempAmount < tilesAmount)
                {
                    tempTileList.Add(tempTile);
                    ++tempAmount;
                }

            } while (tempTileList.Contains(tempTile));

            
        }

        return tempTileList;
    }

    private bool CheckPlacementAvailable(Tile tile)
    {
        int xPos = tile.GetCoord().x;
        int zPos = tile.GetCoord().y;
        
        if(xPos > 0 && xPos < testBoardSizeX-1)
        {
            if (zPos > 0 && zPos < testBoardSizeZ-1)
            {
                
                return true;
            }
            
        }
        
        return false;
    }

    public Tile GetRandomAvailableTile()
    {
        Tile tileToReturn = null;

        do{
            int indexOfTile = Random.Range(0, tilesList.Count);
            tileToReturn = tilesList[indexOfTile];
        }while (tileToReturn.hasObstacle == true || tileToReturn.hasPlayer == true);

        return tileToReturn;
    }


    public Tile GetTileAtPos(Vector2Int Pos)
    {
        foreach (Tile tile in tilesList)
        {
            if (tile.tileRow == Pos.x && tile.tileColumn == Pos.y)
            {
                return tile;
            }
        }

        return null;
    }

    public bool TileSameLineAndSameColumn(Tile tileOne, Tile tileTwo)
    {
        if (tileOne.tileColumn == tileTwo.tileColumn)
        {
            return true;
        }
        if (tileOne.tileRow == tileTwo.tileRow)
        {
            return true;
        }

        return false;
    }

    public List<Tile> GetAffectedTiles(List<Vector2Int> paternOfAttack, Tile tileOfPlayer, Vector2Int rotationOfPlayer)
    {
        List<Vector2Int> currentPaternOfAttack = new List<Vector2Int>();
        List<Tile> currentTilesToReturn = new List<Tile>();

        foreach (Vector2Int pos in paternOfAttack)
        {
            if (rotationOfPlayer.x > 0) // right
            {
                currentPaternOfAttack.Add(SwapValuesOfVector(pos));
            }
            else if(rotationOfPlayer.x < 0) //left
            {
                currentPaternOfAttack.Add(SwapValuesOfVector(InverseVectorSign(pos)));
            }
            else if (rotationOfPlayer.y > 0) //up
            {
                currentPaternOfAttack.Add(pos);
            }
            else if (rotationOfPlayer.y < 0) //down
            {
                currentPaternOfAttack.Add(InverseVectorSign(pos));
            }
        }

        foreach (Vector2Int pos in currentPaternOfAttack)
        {
            Tile currentTile = GetTileAtPos(new Vector2Int(tileOfPlayer.tileRow + pos.x, tileOfPlayer.tileColumn + pos.y));
            if (currentTile != null)
            {
                currentTilesToReturn.Add(currentTile);
            }
        }

        return currentTilesToReturn;
    }

    public Vector2Int InverseVectorSign(Vector2Int vector)
    {
        return vector * (-1);
    }

    public Vector2Int SwapValuesOfVector(Vector2Int vector)
    {
        return new Vector2Int(vector.y, vector.x);
    }

    public List<Vector2Int> GetAllTilesCoords()
    {
        List<Vector2Int> tempList = new List<Vector2Int>();

        foreach (Tile tile in tilesList)
        {
            tempList.Add(new Vector2Int(tile.tileRow, tile.tileColumn));
        }

        return tempList;
    }

    public List<Vector3> GetAllTilesPositions()
    {
        List<Vector3> tempList = new List<Vector3>();

        foreach (Tile tile in tilesList)
        {
            tempList.Add(tile.gameObject.transform.position);
        }

        return tempList;
    }
    
}
