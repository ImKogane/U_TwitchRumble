using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTrap : CommandInGame 
{
    public CommandTrap(Player ownerOfAction) : base(ownerOfAction)
    {
        OwnerPlayer = ownerOfAction;
    }
    
    public override void SubscribeEndToEvent()
    {
        OwnerPlayer.EndOfAttack += EndActionInGame;
    }

    public override void LaunchActionInGame()
    {
        SubscribeEndToEvent();
        Debug.Log("Start Trap Action");

        OwnerPlayer.StartCoroutine(OwnerPlayer.SetupTrapCoroutine());
    }

    public override void DestroyCommand()
    {
        OwnerPlayer.EndOfAttack -= EndActionInGame;
    }
    
    
}
