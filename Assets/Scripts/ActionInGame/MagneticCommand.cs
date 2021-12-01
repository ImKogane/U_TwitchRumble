using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticCommand : CommandInGame
{
    Vector2Int VectorAttirance;
    Tile TyleToAffect;
    Player playerToAffect;

    public MagneticCommand(Player OwnerOfAction, Vector2Int vectorAttirance, Tile tyleToAffect) : base(OwnerOfAction)
    {
        OwnerPlayer = OwnerOfAction;
        VectorAttirance = vectorAttirance;
        TyleToAffect = tyleToAffect;
    }

    public override void SubscribeEndToEvent()
    {
        playerToAffect.playerMovement.EndOfMoving += EndActionInGame;
    }

    public override void LaunchActionInGame()
    {
        playerToAffect = TyleToAffect.currentPlayer;

        SubscribeEndToEvent();

        Tile startTile = OwnerPlayer.CurrentTile;

        //Get player infos.
        PlayerMovement _currentMovementPlayer = TyleToAffect.currentPlayer.playerMovement;

        _currentMovementPlayer.RotatePlayerWithvector(VectorAttirance * -1);

        Debug.Log($"MagneticCommandStart, owner : {OwnerPlayer.name}, vector : {VectorAttirance * -1}, playerAffect : {TyleToAffect.currentPlayer.name}");

        if (VectorAttirance.x != 0) //Left and Right
        {
            //Move the player.
            _currentMovementPlayer.CheckBeforeMoveToATile(BoardManager.Instance.GetTileAtPos(new Vector2Int(TyleToAffect.tileRow + (-VectorAttirance.x), startTile.tileColumn)));
        }
        if (VectorAttirance.y != 0) //Up and Down
        {
            //Move the player.
            _currentMovementPlayer.CheckBeforeMoveToATile(BoardManager.Instance.GetTileAtPos(new Vector2Int(startTile.tileRow, TyleToAffect.tileColumn + (-VectorAttirance.y))));
        }
    }

    public override void DestroyCommand()
    {
        Debug.Log("Destroy CommandMagnetic");
        playerToAffect.playerMovement.EndOfMoving -= EndActionInGame;
    }
}
