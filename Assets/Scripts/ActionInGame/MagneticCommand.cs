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
            return;
        }
        else
        {
            if (_affectedTile.currentPlayer)
            {
                _affectedPlayer = _affectedTile.currentPlayer;
            }
            else
            {
                EndActionInGame();
                return;
            }
        }
        
        //If either the attracting player or the affected player is dead, we stop
        if (_ownerPlayer._isDead || _affectedPlayer._isDead) 
        {
            EndActionInGame();
            return;
        }
        
        //If both players are no longer on the same row or column, we stop
        if (!BoardManager.Instance.TileSameLineAndSameColumn(_ownerPlayer._currentTile, _affectedTile))
        {
            EndActionInGame();
            return;
        }

        SubscribeEndToEvent();

        Tile startTile = _ownerPlayer._currentTile;

        //Get player infos
        PlayerMovement _currentMovementPlayer = _affectedTile.currentPlayer._playerMovementComponent;

        _currentMovementPlayer.RotatePlayerWithvector(_vectorAttirance * -1);

        _vectorAttirance = NormalizeTheVector(_vectorAttirance);

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

    public Vector2Int NormalizeTheVector(Vector2Int vector)
    {
        Vector2Int vectorToReturn = vector;
        
        if (vector.x != 0)
        {
            if (vector.x > 1)
            {
                vectorToReturn.x = 1;
            }
            if (vector.x < -1)
            {
                vectorToReturn.x = -1;
            }
        }
        
        if (vector.y != 0)
        {
            if (vector.y > 1)
            {
                vectorToReturn.y = 1;
            }
            if (vector.y < -1)
            {
                vectorToReturn.y = -1;
            }
        }

        return vectorToReturn;
    }

    //Unsubscribe the method for a safe removal
    public override void DestroyCommand()
    {
        if (_affectedPlayer &&  _affectedPlayer._playerMovementComponent)
        {
            _affectedPlayer._playerMovementComponent.EndOfMoving -= EndActionInGame;
        }

    }
}
