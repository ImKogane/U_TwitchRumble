using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileRow;
    public int tileColumn;

    public bool hasObstacle;
    public bool hasPlayer;
    
    public void SetCoord(Vector2Int newCoord)
    {
        Debug.Log("Tile [" + newCoord.x + "," + newCoord.y + "]");
        tileRow = newCoord.x;
        tileColumn = newCoord.y;
    }

    public Vector2Int GetCoord()
    {
        return new Vector2Int(tileRow, tileColumn);
    }

    public bool GetHasObstacle()
    {
        return hasObstacle;
    }

    public void SetHasObstacle(bool newValue)
    {
        hasObstacle = newValue;
    }
    
}
