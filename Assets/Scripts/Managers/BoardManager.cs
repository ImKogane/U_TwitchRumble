using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Configuration;
using UnityEngine;

public class BoardManager : SingletonMonobehaviour<BoardManager>
{
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _obstaclePrefab;

    private GameObject _gameBoard;
    
    [HideInInspector]
    public List<Tile> _listTiles = new List<Tile>();
    
    private float _tileWidth;
    private float _tileLength;
    
    [SerializeField] private int _boardSizeX;
    [SerializeField] private int _boardSizeZ;
    [SerializeField] private int _holeNumber;
    [SerializeField] private int _obstaclesNumber;

    public GameObject _trapPrefab;
    
    public override bool DestroyOnLoad => true;

    void Start()
    {
        //On sait que cette methode ne devrait pas etre appelée ici, mais manque de temps pour trouver une nouvelle solution. 
        StartGameManager.Instance.LaunchGame(); 
    }

    #region SetUp Board
    public void SetupNewBoard()
    {
        //Set up variable with google sheet datas.
        _holeNumber = GoogleSheetManager.Instance._variablesGetFromSheet[1];
        _obstaclesNumber = GoogleSheetManager.Instance._variablesGetFromSheet[2];

        MeshRenderer tileRenderer = _tilePrefab.GetComponentInChildren<MeshRenderer>();
        
        if (tileRenderer)
        {
            _tileLength = tileRenderer.bounds.size.z;
            _tileWidth = tileRenderer.bounds.size.x;
        }
        
        ResetTiles();
        _gameBoard = new GameObject("GameBoard");
        CreateBoard(_boardSizeX, _boardSizeZ, _holeNumber,_obstaclesNumber);
    }

    public void SetupCustomBoard()
    {
        //Set up variable with google sheet datas.
        _holeNumber = GoogleSheetManager.Instance._variablesGetFromSheet[1];
        _obstaclesNumber = GoogleSheetManager.Instance._variablesGetFromSheet[2];

        MeshRenderer tileRenderer = _tilePrefab.GetComponentInChildren<MeshRenderer>();
        
        if (tileRenderer)
        {
            _tileLength = tileRenderer.bounds.size.z;
            _tileWidth = tileRenderer.bounds.size.x;
        }
        
        _gameBoard = new GameObject("GameBoard");
        ResetTiles();
    }
    
    public void CreateBoard(int sizeX, int sizeZ, int holeNumber, int obstacleNumber)
    {
        #region Basic Board Creation

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                float tempPosZ = j * _tileLength;
                float tempPosX = i * _tileWidth;

                Tile newTile = Instantiate(_tilePrefab, new Vector3(tempPosX, 0, tempPosZ), Quaternion.identity, _gameBoard.transform).GetComponent<Tile>();

                if (newTile)
                {
                    _listTiles.Add(newTile);
                    newTile.SetCoord(new Vector2Int(i, j));
                }
                
            }
        }
        
        #endregion

        #region Holes placement

        List<Tile> tilesWithHole = GetRandomTiles(holeNumber);

        foreach (var tile in tilesWithHole)
        {
            _listTiles.Remove(tile);
            Destroy(tile.gameObject);
        }

        #endregion
        
        #region Obstacle Placement
        
        List<Tile> tilesWithObstacle = GetRandomTiles(obstacleNumber);

        foreach (var tile in tilesWithObstacle)
        {
            tile.SetHasObstacle(true);
            Instantiate(_obstaclePrefab, tile.transform.position, Quaternion.identity, _gameBoard.transform);
        }

        #endregion
    }
    #endregion

    #region Loading Datas
    public void LoadTile(TileData tileData)
    {
       Tile newTile = Instantiate(_tilePrefab, tileData._tileTransform.Position,  Quaternion.Euler(tileData._tileTransform.Rotation), _gameBoard.transform).GetComponent<Tile>();

        newTile.transform.localScale = tileData._tileTransform.Scale;

        newTile.tileColumn = tileData._tileCoords.x;
        newTile.tileRow = tileData._tileCoords.y;

        newTile.hasObstacle = tileData._hasObstacle;
        
        if (newTile.hasObstacle)
        {
            Instantiate(_obstaclePrefab, newTile.transform.position, Quaternion.identity, _gameBoard.transform);
        }

        for (int i = 0; i < tileData._tileTrapCount; i++)
        {
            Trap newTrap = Instantiate(_trapPrefab, newTile.transform.position, Quaternion.identity, _gameBoard.transform).GetComponent<Trap>();
            newTile.trapList.Add(newTrap);
        }
        
        _listTiles.Add(newTile);
    }
    #endregion

    #region Usefull Functions 
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
                tempTileIndex = Random.Range(0, _listTiles.Count);
                tempTile = _listTiles[tempTileIndex];

                if (CheckPlacementAvailable(tempTile) && tempAmount < tilesAmount)
                {
                    tempTileList.Add(tempTile);
                    tempAmount++;
                }

            } while (tempTileList.Contains(tempTile));


        }

        return tempTileList;
    }

    private bool CheckPlacementAvailable(Tile tile)
    {
        int xPos = tile.GetCoord().x;
        int zPos = tile.GetCoord().y;
        
        if(xPos > 0 && xPos < _boardSizeX-1)
        {
            if (zPos > 0 && zPos < _boardSizeZ-1)
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
            int indexOfTile = Random.Range(0, _listTiles.Count);
            tileToReturn = _listTiles[indexOfTile];
        }while (tileToReturn.hasObstacle == true || tileToReturn.hasPlayer == true);

        return tileToReturn;
    }

    public Tile GetTileAtPos(Vector2Int Pos)
    {
        foreach (Tile tile in _listTiles)
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

    public void ResetTiles()
    {
        if (_listTiles.Count < 1) return;
        
        for (int i = _listTiles.Count; i >= 0; i--)
        {
            Destroy(_listTiles[i].gameObject);
        }
    }
    #endregion

}
