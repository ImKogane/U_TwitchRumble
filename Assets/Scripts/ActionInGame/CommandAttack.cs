
public class CommandAttack : CommandInGame
{
    //Constructor
    public CommandAttack(Player OwnerOfAction) : base(OwnerOfAction)
    {
        _ownerPlayer = OwnerOfAction;
    }

    //Subscribe the command to the action ending management method
    public override void SubscribeEndToEvent()
    {
        _ownerPlayer._endOfAttackAction += EndActionInGame;
    }

    //Start the attack animation which has the Attack animation event in it
    public override void LaunchActionInGame()
    {
        SubscribeEndToEvent();
        _ownerPlayer.StartAttackAnimation();
    }

    //Unsubscribe the method for a safe removal
    public override void DestroyCommand()
    {
        _ownerPlayer._endOfAttackAction -= EndActionInGame;
    }
}
