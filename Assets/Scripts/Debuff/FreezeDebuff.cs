using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeDebuff : Debuff
{
    public FreezeDebuff(int newDuration, Player playerOwner) : base(newDuration, playerOwner)
    {
        
    }

    public override void ApplyEffect()
    {
        ownerOfDebuff.playerMovement.canMove = false;
    }

    public override void RemoveEffect()
    {
        ownerOfDebuff.playerMovement.canMove = true;
        base.RemoveEffect();
    }
}
