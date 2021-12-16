using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    [Header("Animations Values")]
    public float _walkMovementSpeed = 1f;
    public float _pushedMovementSpeed = 2f;
    public List<SO_GameEvent> _foostepsEvents;
    
    [Header("Gameplay Values")]
    public bool _canMove = true;
    public bool _canJumpObstacle = false;
    public Quaternion _initialRotation;
    public Vector2Int _rotationOfPlayer;

    [Header("Other Components")]
    public Player _currentPlayer;
    public Animator _animator;
    
    private bool isMoving;
    public Action EndOfMoving;

    public void SetUpPlayerMovement(Player player)
    {
        _currentPlayer = player;
        _initialRotation = transform.rotation;
        RotateDownDirection();
        _canMove = true;
        _animator = GetComponent<Animator>();
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
        _rotationOfPlayer = new Vector2Int(0, 1);
        gameObject.transform.rotation = _initialRotation;
        gameObject.transform.Rotate(0, 180, 0);
    }
    public void RotateDownDirection()
    {
        _rotationOfPlayer = new Vector2Int(0, -1);
        gameObject.transform.rotation = _initialRotation;
    }
    public void RotateRightDirection()
    {
        _rotationOfPlayer = new Vector2Int(1, 0);
        gameObject.transform.rotation = _initialRotation;
        gameObject.transform.Rotate(0, -90, 0);
    }
    public void RotateLeftDirection()
    {
        _rotationOfPlayer = new Vector2Int(-1, 0);
        gameObject.transform.rotation = _initialRotation;
        gameObject.transform.Rotate(0, 90, 0);
    }
    #endregion

    #region Movement
    public void MakeMovement(bool isPushed = false)
    {
        if (!_canMove && !isPushed)
        {
            EndOfMoving.Invoke();
            return;
        }

        if (_currentPlayer._currentTile)
        {
            Debug.Log("Current Tile : [" + _currentPlayer._currentTile.tileRow + "," + _currentPlayer._currentTile.tileColumn + "]");
        }

        Tile NextTile = null;

        //Avancer en X 
        if (_rotationOfPlayer.x != 0)
        {
            NextTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(_currentPlayer._currentTile.tileRow + _rotationOfPlayer.x, _currentPlayer._currentTile.tileColumn));
            CheckBeforeMoveToATile(NextTile, isPushed);
        }
        //Avancer en Z
        else if (_rotationOfPlayer.y != 0)
        {
            NextTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(_currentPlayer._currentTile.tileRow, _currentPlayer._currentTile.tileColumn + _rotationOfPlayer.y));
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
            if (_canJumpObstacle)
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
        if (_rotationOfPlayer.x != 0)
        {
            if (BoardManager.Instance.GetTileAtPos(new Vector2Int(_currentPlayer._currentTile.tileRow + _rotationOfPlayer.x * 2, _currentPlayer._currentTile.tileColumn)))
            {
                NextTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(_currentPlayer._currentTile.tileRow + _rotationOfPlayer.x * 2, _currentPlayer._currentTile.tileColumn));
                GoToATile(NextTile);
                return;
            }
        }
        //Sauter en Z
        else if (_rotationOfPlayer.y != 0)
        {
            if (BoardManager.Instance.GetTileAtPos(new Vector2Int(_currentPlayer._currentTile.tileRow, _currentPlayer._currentTile.tileColumn + _rotationOfPlayer.y * 2)))
            {
                NextTile = BoardManager.Instance.GetTileAtPos(new Vector2Int(_currentPlayer._currentTile.tileRow , _currentPlayer._currentTile.tileColumn + _rotationOfPlayer.y * 2));
                GoToATile(NextTile);
                return;
                
            }
        }
    }

    public void ResetMyTile()
    {
        _currentPlayer._currentTile.hasPlayer = false;
        _currentPlayer._currentTile.currentPlayer = null;
        _currentPlayer._currentTile = null;
    }

    public void SetNewTile(Tile nextTile)
    {
        _currentPlayer._currentTile = nextTile;
        _currentPlayer._currentTile.currentPlayer = _currentPlayer;
        _currentPlayer._currentTile.hasPlayer = true;
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
            StartCoroutine(nextDestination.trapList[i].Trigger(_currentPlayer));
        }

        if (_currentPlayer._currentMoveBuff != null)
        {
            if (!isPushed)
            {
                _currentPlayer._currentMoveBuff.ApplyMoveBuff(_currentPlayer);
            }
        }

        EndOfMoving.Invoke();
    }

    private IEnumerator FallInWaterCoroutine()
    {
        Debug.Log("Aucune Tile detectée, vous plongez dans l'océan.");

        float offset = 5;

        //Avancer jusqu'au milieu du trou.
        if (_rotationOfPlayer.x != 0)
        {
            transform.DOMove(new Vector3(transform.position.x + (offset * _rotationOfPlayer.x), transform.position.y, transform.position.z), _walkMovementSpeed);
        }
        if (_rotationOfPlayer.y != 0)
        {
            transform.DOMove(new Vector3(transform.position.x , transform.position.y, transform.position.z + (offset * _rotationOfPlayer.y)), _walkMovementSpeed);
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
        
        _animator.SetBool("IsFalling", true);
        
        //Attendre d'etre dans l'eau.
        yield return new WaitForSeconds(delayGoUp + delayGoDown);

        _currentPlayer.KillPlayer();
    }
    #endregion

    public void PlayFoostepSound()
    {
        int index = Random.Range(0, _foostepsEvents.Count - 1);
        _foostepsEvents[index].Raise();
    }
}
