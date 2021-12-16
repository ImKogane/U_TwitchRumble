using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandChoice : CommandInGame
{
    SO_Choice _choice;

    //Constructor
    public CommandChoice(Player ownerOfAction, SO_Choice choiceOfCommand) : base(ownerOfAction)
    {
        _ownerPlayer = ownerOfAction;
        _choice = choiceOfCommand;
    }
    
    //Give to player a new choice effect according to the Scriptable Object
    public override void LaunchActionInGame()
    {
        _ownerPlayer.ReceiveAChoice(_choice);
        EndActionInGame();
    }
}
