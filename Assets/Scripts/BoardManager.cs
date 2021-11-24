using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject obstaclePrefab;

    private GameObject gameBoard;
    
    private List<Tile> tilesList = new List<Tile>();
    
    private float tileWidth;
    private float tileLength;
    
    [SerializeField] private int testBoardSizeX;
    [SerializeField] private int testBoardSizeZ;
    [SerializeField] private int testHoleNumber;
    [SerializeField] private int testObstaclesNumber;

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

    void Start()
    {
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
        
        gameBoard = new GameObject("GameBoard");

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                float tempPosZ = i * tileLength;
                float tempPosX = j * tileWidth;

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
    }

    private List<Tile> GetRandomTiles(int tilesAmount)
    {
        int tempAmount = 0;

        List<Tile> tempTileList = new List<Tile>();
        
        while (tempAmount < tilesAmount)
        {
            int tempTileIndex = Random.Range(0, tilesList.Count);

            Tile tempTile = tilesList[tempTileIndex];
            
            tempTileList.Add(tempTile);
            
            ++tempAmount;
        }

        return tempTileList;
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
    
}
