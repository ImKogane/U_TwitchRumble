using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMoving : CommandInGame
{
    Vector2Int _moveDirection;
    private bool _isPushed; //When the player gets pushed or pull because of an enemy buff effect
    
    
    //Constructor
    public CommandMoving(Player OwnerOfAction, Vector2Int moveDirection, bool isPushedValue = false) : base(OwnerOfAction)
    {
        _ownerPlayer = OwnerOfAction;
        _moveDirection = moveDirection;
        _isPushed = isPushedValue;
    }

    //Subscribe the command to the action ending management method
    public override void SubscribeEndToEvent()
    {
        _ownerPlayer._playerMovementComponent.EndOfMoving += EndActionInGame;
    }

    //Rotate the player so he's facing the direction he's moving to
    public override void LaunchActionInGame()
    {
        if (_ownerPlayer._isDead) //Safety check
        {
            EndActionInGame();
        }
        
        SubscribeEndToEvent();
        _ownerPlayer._playerMovementComponent.RotatePlayerWithvector(_moveDirection);
        _ownerPlayer._playerMovementComponent.MakeMovement(_isPushed);
    }

    //Unsubscribe the method for a safe removal
    public override void DestroyCommand()
    {
        _ownerPlayer._playerMovementComponent.EndOfMoving -= EndActionInGame;
    }
}
