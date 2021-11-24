using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttack : ActionInGame
{
    public ActionAttack(Player OwnerOfAction) : base(OwnerOfAction)
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
        OwnerPlayer.Attack();
    }
}
