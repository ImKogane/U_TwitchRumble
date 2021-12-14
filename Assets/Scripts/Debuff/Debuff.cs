using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff
{
    public int duration;
    public Player ownerOfDebuff;
    
    public Debuff(int newDuration, Player playerOwner)
    {
        duration = newDuration;
        ownerOfDebuff = playerOwner;
    }

    public virtual void OnPlayerReceiveDebuff()
    {
        
    }
    
    public virtual void ApplyEffect()
    {
        
    }

    public virtual void RemoveEffect()
    {
        if (ownerOfDebuff.debuffList.Contains(this))
        {
            ownerOfDebuff.debuffList.Remove(this);
        }
    }
    
}
