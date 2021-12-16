

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
        CommandManager.Instance.ManageEndOfCommand(this);
    }

    public virtual void DestroyCommand()
    {

    }
}
