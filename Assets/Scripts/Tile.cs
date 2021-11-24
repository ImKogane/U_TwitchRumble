using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int tileRow;
    private int tileColumn;

    private bool hasObstacle;
    
    public void SetCoord(Vector2Int newCoord)
    {
        tileRow = newCoord.x;
        tileColumn = newCoord.y;
    }

    public Vector2 GetCoord()
    {
        return new Vector2(tileRow, tileColumn);
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
