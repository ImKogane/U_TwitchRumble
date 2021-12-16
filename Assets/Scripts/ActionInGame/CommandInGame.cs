using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class CommandInGame 
{
    public Player OwnerPlayer = null;

    public CommandInGame(Player OwnerOfAction)
    {
        OwnerPlayer = OwnerOfAction;
    }

    public virtual void LaunchActionInGame()
    {
       
    }

    public virtual void SubscribeEndToEvent()
    {

    }

    public virtual void EndActionInGame()
    {
        GlobalManager.Instance.ManageEndOfCommand(this);
    }

    public virtual void DestroyCommand()
    {

    }
}
