

public class CommandInGame 
{
    public Player _ownerPlayer = null;

    //Constructor
    public CommandInGame(Player OwnerOfAction)
    {
        _ownerPlayer = OwnerOfAction;
    }

    //What the command will do when called
    public virtual void LaunchActionInGame()
    {

    }

    //To which Action the Command is asked to finish itself
    public virtual void SubscribeEndToEvent()
    {

    }

    //The Global Manager choose what to do when the command ends
    public virtual void EndActionInGame()
    {
        CommandManager.Instance.ManageEndOfCommand(this);
    }

    //What does the command needs to do when destroyed
    public virtual void DestroyCommand()
    {

    }
}
