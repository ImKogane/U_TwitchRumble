using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Tile currentTile;
    private Tile destinationTile;
    private bool isMoving;

    [SerializeField] private float movePrecision;

    public void SetDestination(Tile nextDestination)
    {
        destinationTile = nextDestination;
        isMoving = true;
    }

    public Tile GetDestinationTile()
    {
        return destinationTile;
    }
    
    public void SetCurrentTile(Tile newTile)
    {
        currentTile = newTile;
    }

    public Tile GetCurrentTile()
    {
        return currentTile;
    }
    
    public void HandleMovement(float deltaTime)
    {
/*        if (!isMoving) return;
        
        transform.Translate(destinationTile.transform.position);

        if (Vector3.Distance(transform.position, destinationTile.transform.position) < movePrecision)
        {
            isMoving = false;
        }*/
    }

}
