using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMoving : CommandInGame
{
    Vector2Int DirectionOfMove;

    private bool isPushed;
    
    public CommandMoving(Player OwnerOfAction, Vector2Int direction, bool isPushedValue = false) : base(OwnerOfAction)
    {
        OwnerPlayer = OwnerOfAction;
        DirectionOfMove = direction;
        isPushed = isPushedValue;
    }

    public override void SubscribeEndToEvent()
    {
        OwnerPlayer.playerMovement.EndOfMoving += EndActionInGame;
    }

    public override void LaunchActionInGame()
    {
        if (OwnerPlayer.isDead)
        {
            EndActionInGame();
        }
        SubscribeEndToEvent();
        Debug.Log("Start moving Action");
        OwnerPlayer.playerMovement.RotatePlayerWithvector(DirectionOfMove);
        OwnerPlayer.playerMovement.MakeMovement(isPushed);
    }

    public override void DestroyCommand()
    {
        OwnerPlayer.playerMovement.EndOfMoving -= EndActionInGame;
    }
}
