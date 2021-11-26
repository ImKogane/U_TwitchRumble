using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public Action EndOfMoving;

    public int MovmentSeconds;

    private Tile currentTile;
    private Tile destinationTile;
    private bool isMoving;

    public Quaternion InitialRotation;
    public Vector2Int RotationOfPlayer;

    public Player CurrentPlayer;

    bool UpdateRotOfUI = false;

    [SerializeField] private float movePrecision;

    private void Start()
    {
        //CurrentPlayer = GetComponent<Player>();
        InitialRotation = transform.rotation;
        RotateDownDirection();
        EndOfMoving += () => { UpdateRotOfUI = false; };
    }

    #region RotatePlayer

    public void RotatePlayer(EnumClass.Direction DirectionOfMovement)
    {
        switch (DirectionOfMovement)
        {
            case EnumClass.Direction.Up:
                RotateUpDirection();
                break;
            case EnumClass.Direction.Down:
                RotateDownDirection();
                break;
            case EnumClass.Direction.Right:
                RotateRightDirection();
                break;
            case EnumClass.Direction.Left:
                RotateLeftDirection();
                break;
            default:
                break;
        }
    }

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

    private void Update()
    {
        if (UpdateRotOfUI)
        {
            CurrentPlayer.UpdatePlayerCanvas();
        }
    }

    #region Movement
    public void MakeMovement()
    {
        UpdateRotOfUI = true; 

        if (CurrentPlayer.CurrentTile)
        {
            Debug.Log("Current Tile : [" + CurrentPlayer.CurrentTile.tileRow + "," + CurrentPlayer.CurrentTile.tileColumn + "]");
        }

        Tile NextTile = null;

        //Avancer en X 
        if (RotationOfPlayer.x != 0)
        {
            Debug.Log("board manager : " + BoardManager.Instance);
            if (NextTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentPlayer.CurrentTile.tileRow + RotationOfPlayer.x, CurrentPlayer.CurrentTile.tileColumn)))
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
            if (NextTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentPlayer.CurrentTile.tileRow, CurrentPlayer.CurrentTile.tileColumn + RotationOfPlayer.y)))
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

    private void MoveToATile(Tile nextTile)
    {
        Debug.Log("Cellule ciblée : [" + nextTile.tileRow + "," + nextTile.tileColumn + "]");

        if (nextTile.hasObstacle)
        {
            Debug.Log("Cellule ciblée est occupé par un obstacle.");
            EndOfMoving.Invoke();
            return;
        }
        if (nextTile.hasPlayer)
        {
            Debug.Log("Cellule ciblée est occupé par un joueur.");
            EndOfMoving.Invoke();
            return;
        }

        Debug.Log("Tile detecté, le mouvement peut etre fait !");

        ResetMyTile();

        StartCoroutine(DotweenMovment(nextTile));

        SetNewTile(nextTile);
    }

    public void ResetMyTile()
    {
        CurrentPlayer.CurrentTile.hasPlayer = false;
        CurrentPlayer.CurrentTile.currentPlayer = null;
        CurrentPlayer.CurrentTile = null;
    }

    public void SetNewTile(Tile nextTile)
    {
        CurrentPlayer.CurrentTile = nextTile;
        CurrentPlayer.CurrentTile.currentPlayer = CurrentPlayer;
        CurrentPlayer.CurrentTile.hasPlayer = true;
    }


    private IEnumerator DotweenMovment(Tile nextDestination)
    {
        transform.DOMove(nextDestination.transform.position, MovmentSeconds);

        yield return new WaitForSeconds(MovmentSeconds);
        EndOfMoving.Invoke();
    }

    private void FallInWater()
    {
        Debug.Log("Aucune Tile detectée, vous plongez dans l'océan.");
        EndOfMoving.Invoke();
    }
    #endregion

    #region Getters/Setters
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
    #endregion
}
