using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningDebuff : Debuff
{
    public int damages = 15;

    public BurningDebuff(int newDuration, Player playerOwner) : base(newDuration, playerOwner)
    {
    }

    public override void OnPlayerReceiveDebuff()
    {
        //Play big FX
    }

    public override void ApplyEffect()
    {
       ownerOfDebuff.ReceiveDamage(damages);
    }

}
