
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
        EndActionInGame();
    }
}
