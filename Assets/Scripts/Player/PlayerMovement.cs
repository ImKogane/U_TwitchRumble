using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.Rendering.UI;

public class PlayerMovement : MonoBehaviour
{
    public Action EndOfMoving;

    public float _walkMovementSpeed = 1f;
    public float _pushedMovementSpeed = 2f;

    private bool isMoving;
    [NonSerialized]
    public bool canMove = true;

    public Quaternion InitialRotation;
    public Vector2Int RotationOfPlayer;

    public Player CurrentPlayer;

    [SerializeField] private float movePrecision;

    [NonSerialized]
    public bool CanJumpObstacle = false;

    bool UpdateRotOfUI = false;

    public Animator _animator;
    
    private void Start()
    {
        InitialRotation = transform.rotation;
        RotateDownDirection();
        EndOfMoving += () => { UpdateRotOfUI = false; };
        canMove = true;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CurrentPlayer.UpdatePlayerCanvas();
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

    public void RotatePlayerWithvector(Vector2Int VecteurDirection)
    {
        if (VecteurDirection.x == 1)
        {
            RotateRightDirection();
        }
        else if (VecteurDirection.x == -1)
        {
            RotateLeftDirection();
        }
        else if (VecteurDirection.y == 1)
        {
            RotateUpDirection();
        }
        else if (VecteurDirection.y == -1)
        {
            RotateDownDirection();
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

    #region Movement
    public void MakeMovement(bool isPushed = false)
    {
        if (!canMove && !isPushed)
        {
            EndOfMoving.Invoke();
            return;
        }

        UpdateRotOfUI = true;
        
        if (CurrentPlayer.CurrentTile)
        {
            Debug.Log("Current Tile : [" + CurrentPlayer.CurrentTile.tileRow + "," + CurrentPlayer.CurrentTile.tileColumn + "]");
        }

        Tile NextTile = null;

        //Avancer en X 
        if (RotationOfPlayer.x != 0)
        {
            NextTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentPlayer.CurrentTile.tileRow + RotationOfPlayer.x, CurrentPlayer.CurrentTile.tileColumn));
            CheckBeforeMoveToATile(NextTile, isPushed);
        }
        //Avancer en Z
        else if (RotationOfPlayer.y != 0)
        {
            NextTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentPlayer.CurrentTile.tileRow, CurrentPlayer.CurrentTile.tileColumn + RotationOfPlayer.y));
            CheckBeforeMoveToATile(NextTile, isPushed);
        }
        else
        {
            Debug.Log("Le joueur est pas orienté en X ou en Z");
        }
    }

    public void CheckBeforeMoveToATile(Tile nextTile, bool isPushed = false)
    {
        if (nextTile == null)
        {
            StartCoroutine(FallInWaterCoroutine());
            return;
        }

        Debug.Log("Cellule ciblée : [" + nextTile.tileRow + "," + nextTile.tileColumn + "]");

        if (nextTile.hasObstacle)
        {
            Debug.Log("Cellule ciblée est occupé par un obstacle.");
            if (CanJumpObstacle)
            {
                Debug.Log("Vous sautez au dessus d'un obstacle.");
                JumpAnObstacle();
            }
            EndOfMoving.Invoke();
            return;
        }

        if (nextTile.hasPlayer)
        {
            Debug.Log("Cellule ciblée est occupé par un joueur.");
            EndOfMoving.Invoke();
            return;
        }

        GoToATile(nextTile, isPushed);
    }

    private void GoToATile(Tile nextTile, bool isPushed = false)
    {
        Debug.Log("Tile detecté, le mouvement peut etre fait !");

        ResetMyTile();

        StartCoroutine(DotweenMovment(nextTile, isPushed));

        SetNewTile(nextTile);
    }

    public void JumpAnObstacle()
    {
        Tile NextTile = null;

        //Sauter en X
        if (RotationOfPlayer.x != 0)
        {
            if (BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentPlayer.CurrentTile.tileRow + RotationOfPlayer.x * 2, CurrentPlayer.CurrentTile.tileColumn)))
            {
                NextTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentPlayer.CurrentTile.tileRow + RotationOfPlayer.x * 2, CurrentPlayer.CurrentTile.tileColumn));
                GoToATile(NextTile);
                return;
            }
        }
        //Sauter en Z
        else if (RotationOfPlayer.y != 0)
        {
            if (BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentPlayer.CurrentTile.tileRow, CurrentPlayer.CurrentTile.tileColumn + RotationOfPlayer.y * 2)))
            {
                NextTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(CurrentPlayer.CurrentTile.tileRow , CurrentPlayer.CurrentTile.tileColumn + RotationOfPlayer.y * 2));
                GoToATile(NextTile);
                return;
                
            }
        }
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


    private IEnumerator DotweenMovment(Tile nextDestination, bool isPushed)
    {
        float moveTime;
        Ease movementEase;
        
        if (!isPushed)
        {
            _animator.SetBool("IsWalking", true);
            moveTime = _walkMovementSpeed;
            movementEase = Ease.InOutSine;
        }
        else
        {
            _animator.SetBool("IsPushed", true);
            moveTime = _pushedMovementSpeed;
            movementEase = Ease.OutQuint;
        }

        transform.DOMove(nextDestination.transform.position, moveTime, false).SetEase(movementEase);
        yield return new WaitForSeconds(moveTime);
        
        if (!isPushed)
        {
            _animator.SetBool("IsWalking", false);
        }
        else
        {
            _animator.SetBool("IsPushed", false);
        }

        for (int i = 0; i < nextDestination.trapList.Count; i++)
        {
            nextDestination.trapList[i].Trigger(CurrentPlayer);
        }

        if (CurrentPlayer.playerMoveBuff != null)
        {
            CurrentPlayer.playerMoveBuff.ApplyMoveBuff(CurrentPlayer);
        }

        EndOfMoving.Invoke();
    }

    private IEnumerator FallInWaterCoroutine()
    {
        Debug.Log("Aucune Tile detectée, vous plongez dans l'océan.");

        float offset = 5;

        //Avancer jusqu'au milieu du trou.
        if (RotationOfPlayer.x != 0)
        {
            transform.DOMove(new Vector3(transform.position.x + (offset * RotationOfPlayer.x), transform.position.y, transform.position.z), _walkMovementSpeed);
        }
        if (RotationOfPlayer.y != 0)
        {
            transform.DOMove(new Vector3(transform.position.x , transform.position.y, transform.position.z + (offset * RotationOfPlayer.y)), _walkMovementSpeed);
        }

        //Attendre d'etre au milieu du trou. 
        yield return new WaitForSeconds(_walkMovementSpeed);

        int offsetGoUp = 2;
        int offsetGoDown = 8;
        float delayGoUp = 0.2f;
        float delayGoDown = 1.5f;

        //Jouer l'animation pour tomber dans l'eau.
        Sequence fallSequence = DOTween.Sequence()
        .Append(transform.DOMove(transform.position + Vector3.up * offsetGoUp, delayGoUp))
        .Append(transform.DOMove(transform.position - Vector3.up * offsetGoDown, delayGoDown));
        
        _animator.SetTrigger("IsFalling");
        
        //Attendre d'etre dans l'eau.
        yield return new WaitForSeconds(delayGoUp + delayGoDown);

        CurrentPlayer.isDead = true;

        EndOfMoving.Invoke();

        CurrentPlayer.KillPlayer();
    }
    #endregion
}
