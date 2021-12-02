using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandChoice : CommandInGame
{
    SO_Choice choice;

    public CommandChoice(Player ownerOfAction, SO_Choice choiceOfCommand) : base(ownerOfAction)
    {
        OwnerPlayer = ownerOfAction;
        choice = choiceOfCommand;
    }
    public override void LaunchActionInGame()
    {
        OwnerPlayer.ReceiveAChoice(choice);
        Debug.Log($"{OwnerPlayer} make a choice of : {choice}");
        EndActionInGame();
    }
}
