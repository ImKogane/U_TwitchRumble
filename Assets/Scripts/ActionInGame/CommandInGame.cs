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
        if (GlobalManager.Instance.ListCommandsInGame.Count > 0 && GlobalManager.Instance.ListCommandsInGame[0] == this) //Somme nous bien l'action a la base de la liste
        {
            GlobalManager.Instance.ListCommandsInGame.Remove(this); //On s'enleve de la liste. 

            if (GlobalManager.Instance.ListCommandsInGame.Count > 0)
            {
                GlobalManager.Instance.ListCommandsInGame[0].LaunchActionInGame(); //On lance la prochaine action. 

                Debug.Log("Next Action");
            }
            else
            {
                GlobalManager.Instance.EndActionTurn();
            }
        }
    }

    public virtual void DestroyCommand()
    {

    }
}
