using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMoving : ActionInGame
{
    EnumClass.Direction DirectionOfMove;

    public ActionMoving(Player OwnerOfAction, EnumClass.Direction direction) : base(OwnerOfAction)
    {
        OwnerPlayer = OwnerOfAction;
        DirectionOfMove = direction;
    }

    public override void SubscribeEndToEvent()
    {
        OwnerPlayer.playerMovement.EndOfMoving += EndActionInGame;
    }

    public override void LaunchActionInGame()
    {
        SubscribeEndToEvent();
        Debug.Log("Start moving Action");
        OwnerPlayer.playerMovement.RotatePlayer(DirectionOfMove);
        OwnerPlayer.playerMovement.MakeMovement();
    }
}
