
public class BurningDebuff : Debuff
{
    public int _damages = 15;

    //Contructor
    public BurningDebuff(int newDuration, Player playerOwner) : base(newDuration, playerOwner)
    {
        
    }

    //Trigger fire FX
    public override void OnPlayerReceiveDebuff()
    {
        
    }

    //Apply burn FX and damages
    public override void ApplyEffect()
    {
       _debuffVictim.ReceiveDamage(_damages);
    }

}
