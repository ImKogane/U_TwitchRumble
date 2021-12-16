
public class CommandTrap : CommandInGame 
{
    
    //Constructor
    public CommandTrap(Player ownerOfAction) : base(ownerOfAction)
    {
        _ownerPlayer = ownerOfAction;
    }
    
    //Subscribe the command to the action ending management method
    public override void SubscribeEndToEvent()
    {
        _ownerPlayer._endOfAttackAction += EndActionInGame;
    } 
    
    //The player play the trap installation animation during the SetupTrapCoroutine
    public override void LaunchActionInGame()
    {
        SubscribeEndToEvent();

        _ownerPlayer.StartCoroutine(_ownerPlayer.SetupTrapCoroutine());
    }

    //Unsubscribe the method for a safe removal
    public override void DestroyCommand()
    {
        _ownerPlayer._endOfAttackAction -= EndActionInGame;
    }
    
    
}
