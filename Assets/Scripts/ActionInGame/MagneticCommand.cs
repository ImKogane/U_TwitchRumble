using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticCommand : CommandInGame
{
    Vector2Int _vectorAttirance;
    Tile _affectedTile;
    Player _affectedPlayer;

    //Constructor
    public MagneticCommand(Player OwnerOfAction, Vector2Int vectorAttirance, Tile affectedTile) : base(OwnerOfAction)
    {
        _ownerPlayer = OwnerOfAction;
        _vectorAttirance = vectorAttirance;
        _affectedTile = affectedTile;
    }

    //Subscribe the command to the action ending management method
    public override void SubscribeEndToEvent()
    {
        _affectedPlayer._playerMovementComponent.EndOfMoving += EndActionInGame;
    }

    public override void LaunchActionInGame()
    {
        //If the target player is no longer on the tile, we stop
        if (_affectedTile.currentPlayer == null)
        {
            EndActionInGame();
        }
        else
        {
            _affectedPlayer = _affectedTile.currentPlayer;
        }
        
        //If either the attracting player or the affected player is dead, we stop
        if (_ownerPlayer._isDead || _affectedPlayer._isDead) 
        {
            EndActionInGame();
        }
        
        //If both players are no longer on the same row or column, we stop
        if (!BoardManager.Instance.TileSameLineAndSameColumn(_ownerPlayer._currentTile, _affectedTile))
        {
            EndActionInGame();
        }

        SubscribeEndToEvent();

        Tile startTile = _ownerPlayer._currentTile;

        //Get player infos
        PlayerMovement _currentMovementPlayer = _affectedTile.currentPlayer._playerMovementComponent;

        _currentMovementPlayer.RotatePlayerWithvector(_vectorAttirance * -1);
        
        if (_vectorAttirance.x != 0) //Left and Right directions
        {
            //Move the player with a pushed effect
            _currentMovementPlayer.CheckBeforeMoveToATile(BoardManager.Instance.GetTileAtPos(new Vector2Int(_affectedTile.tileRow + (-_vectorAttirance.x), startTile.tileColumn)), true);
        }
        if (_vectorAttirance.y != 0) //Up and Down directions
        {
            //Move the player with a pushed effect
            _currentMovementPlayer.CheckBeforeMoveToATile(BoardManager.Instance.GetTileAtPos(new Vector2Int(startTile.tileRow, _affectedTile.tileColumn + (-_vectorAttirance.y))), true);
        }
    }

    //Unsubscribe the method for a safe removal
    public override void DestroyCommand()
    {
        _affectedPlayer._playerMovementComponent.EndOfMoving -= EndActionInGame;
    }
}
