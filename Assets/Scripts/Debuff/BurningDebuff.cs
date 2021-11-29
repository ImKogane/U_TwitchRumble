using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningDebuff : Debuff
{
    public int damages;
    
    public BurningDebuff(int newDuration, Player playerOwner, int burnDamages) : base(newDuration, playerOwner)
    {
        damages = burnDamages;
    }

    public override void ApplyEffect()
    {
       ownerOfDebuff.ReceiveDamage(damages);
    }

}
