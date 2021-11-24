using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    [SerializeField] private GameObject tilePrefab;

    private List<Tile> tilesList = new List<Tile>();
    
    private float tileWidth;
    private float tileLength;
    
    [SerializeField] private int testBoardSize;
    [SerializeField] private int testHoleNumber;
    [SerializeField] private int testObstaclesNumber;
    
    void Start()
    {
        MeshRenderer tileRenderer = tilePrefab.GetComponentInChildren<MeshRenderer>();
        
        if (tileRenderer)
        {
            tileLength = tileRenderer.bounds.size.z;
            tileWidth = tileRenderer.bounds.size.x;
        }
        
        CreateBoard(testBoardSize, testHoleNumber,testObstaclesNumber);
    }

    public void CreateBoard(int size, int holeNumber, int obstacleNumber)
    {
        #region Basic Board Creation
        
        GameObject board = new GameObject("GameBoard");

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float tempPosZ = i * tileLength;
                float tempPosX = j * tileWidth;

                Tile newTile = Instantiate(tilePrefab, new Vector3(tempPosX, 0, tempPosZ), Quaternion.identity, board.transform).GetComponent<Tile>();

                if (newTile)
                {
                    tilesList.Add(newTile);
                    newTile.SetCoord(new Vector2(i, j));
                }
                
            }
        }
        
        #endregion

        #region Holes placement
        
        int holePlaced = 0;

        while (holePlaced < holeNumber)
        {
            int tempTileIndex = Random.Range(0, (int)tileLength);
            Tile tempTile = tilesList[tempTileIndex];

            tilesList.Remove(tempTile);
            Destroy(tempTile.gameObject);

            ++holePlaced;
        }

        #endregion
    }
}
