using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDeath : CommandInGame
{
    public CommandDeath(Player OwnerOfAction) : base(OwnerOfAction)
    {
        OwnerPlayer = OwnerOfAction;
    }

    public override void SubscribeEndToEvent()
    {
        OwnerPlayer.EndOfDeath += EndActionInGame;
    }

    public override void LaunchActionInGame()
    {
        SubscribeEndToEvent();
        Debug.Log("A Player is Dying");

        OwnerPlayer.StartCoroutine(OwnerPlayer.DeathCoroutine());
    }

    public override void DestroyCommand()
    {
        OwnerPlayer.EndOfDeath -= EndActionInGame;
    }
}
