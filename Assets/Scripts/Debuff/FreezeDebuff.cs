public class FreezeDebuff : Debuff
{
    //Constructor
    public FreezeDebuff(int newDuration, Player playerOwner) : base(newDuration, playerOwner)
    {
        
    }

    //Trigger FX + the player can't move (but still rotate) anymore
    public override void OnPlayerReceiveDebuff()
    {
        _debuffVictim._playerMovementComponent.canMove = false;
    }

    //Refresh the movement freezing boolean
    public override void ApplyEffect()
    {
        _debuffVictim._playerMovementComponent.canMove = false;
        //Play small FX
    }

    //The victim can move freely again
    public override void RemoveEffect()
    {
        _debuffVictim._playerMovementComponent.canMove = true;
        base.RemoveEffect();
    }
}
