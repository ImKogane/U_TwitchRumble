using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileRow;
    public int tileColumn;

    public bool hasObstacle;
    public bool hasPlayer;

    public Player currentPlayer;

    public List<Trap> trapList;

    [Header("Colors")]
    public List<Color> AttackColorChangesList;
    public float waitBetweenEachColor;
    
    public void SetCoord(Vector2Int newCoord)
    {
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

    public IEnumerator ReturnToClassicColor()
    {
        MeshRenderer meshRend = gameObject.GetComponentInChildren<MeshRenderer>();

        foreach (var item in AttackColorChangesList)
        {
            meshRend.material.SetColor("_BaseColor", item);
            yield return new WaitForSeconds(waitBetweenEachColor);
        }
    }
    
}
