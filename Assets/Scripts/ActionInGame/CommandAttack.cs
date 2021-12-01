using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAttack : CommandInGame
{
    public CommandAttack(Player OwnerOfAction) : base(OwnerOfAction)
    {
        OwnerPlayer = OwnerOfAction;
    }

    public override void SubscribeEndToEvent()
    {
        OwnerPlayer.EndOfAttack += EndActionInGame;
    }

    public override void LaunchActionInGame()
    {
        SubscribeEndToEvent();
        Debug.Log("Start attack Action");
        OwnerPlayer.StartCoroutine(OwnerPlayer.StartAttackCoroutine());
    }

    public override void DestroyCommand()
    {
        OwnerPlayer.EndOfAttack -= EndActionInGame;
    }
}
