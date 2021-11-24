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
        OwnerPlayer.playerMovement.EndOfMoving += EndActionInGame; 
    }

    public override void LaunchActionInGame()
    {
        Debug.Log("Start moving Action");
        OwnerPlayer.playerMovement.RotatePlayer(DirectionOfMove);
        OwnerPlayer.playerMovement.MakeMovement();
    }
}
