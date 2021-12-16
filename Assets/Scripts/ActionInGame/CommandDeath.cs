using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDeath : CommandInGame
{
    //Constructor
    public CommandDeath(Player OwnerOfAction) : base(OwnerOfAction)
    {
        _ownerPlayer = OwnerOfAction;
    }

    //Subscribe the command to the action ending management method
    public override void SubscribeEndToEvent()
    {
        _ownerPlayer._endOfDeathAction += EndActionInGame;
    }

    //Calls the animation coroutine which lead to the player removal from game
    public override void LaunchActionInGame()
    {
        SubscribeEndToEvent();
        _ownerPlayer.StartCoroutine(_ownerPlayer.DeathCoroutine());
    }

    //Unsubscribe the method for a safe removal
    public override void DestroyCommand()
    {
        _ownerPlayer._endOfDeathAction -= EndActionInGame;
    }
}
