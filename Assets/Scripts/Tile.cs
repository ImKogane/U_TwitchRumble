using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int xCoord;
    private int zCoord;

    public void SetCoord(Vector2 newCoord)
    {
        xCoord = (int)newCoord.x;
        zCoord = (int)newCoord.y;
    }

    public Vector2 GetCoord()
    {
        return new Vector2(xCoord, zCoord);
    }
    
}
