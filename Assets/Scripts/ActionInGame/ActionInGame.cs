using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ActionInGame 
{
    protected Player OwnerPlayer = null;

    public ActionInGame(Player OwnerOfAction)
    {
        OwnerPlayer = OwnerOfAction;
    }

    public virtual void LaunchActionInGame()
    {
       
    }

    public virtual void EndActionInGame()
    {
        if (GlobalManager.Instance.ListActionsInGame[0] == this)
        {
            GlobalManager.Instance.ListActionsInGame.Remove(this);
            if (GlobalManager.Instance.ListActionsInGame.Count > 0)
            {
                GlobalManager.Instance.ListActionsInGame[0].LaunchActionInGame();
                Debug.Log("Next Action");
            }
        }
    }
}
