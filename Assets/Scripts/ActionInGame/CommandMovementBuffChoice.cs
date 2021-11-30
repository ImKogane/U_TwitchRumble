using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMovementBuffChoice : CommandInGame
{
    private EnumClass.MovementBuffType movementBuffType;
    
    public CommandMovementBuffChoice(Player ownerOfAction, EnumClass.MovementBuffType newMovementBuffType) : base(ownerOfAction)
    {
        OwnerPlayer = ownerOfAction;
        movementBuffType = newMovementBuffType;
    }
    public override void LaunchActionInGame()
    {
        OwnerPlayer.playerMoveBuff = MovementBuffFactory.CreateMovementBuff(movementBuffType, OwnerPlayer);
        Debug.Log("New Player's Movement Buff : " + OwnerPlayer.playerMoveBuff.GetType());
        EndActionInGame();
    }
}
