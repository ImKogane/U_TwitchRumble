
public class Debuff
{
    
    public int _duration;
    public Player _debuffVictim;
    
    //Constructor
    public Debuff(int newDuration, Player newVictimOfDebuff)
    {
        _duration = newDuration;
        _debuffVictim = newVictimOfDebuff;
    }

    //Called when the payer just get a debuff from a weapon hit
    public virtual void OnPlayerReceiveDebuff()
    {
        
    }
    
    //Called at the start of each Action phase to apply additionnal effect if needed
    public virtual void ApplyEffect()
    {
        
    }

    //Remove this debuff from the player's debuff List
    public virtual void RemoveEffect()
    {
        if (_debuffVictim._debuffList.Contains(this))
        {
            _debuffVictim._debuffList.Remove(this);
        }
    }
    
}
