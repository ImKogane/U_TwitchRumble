using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeDebuff : Debuff
{
    public FreezeDebuff(int newDuration, Player playerOwner) : base(newDuration, playerOwner)
    {
        
    }

    public override void OnPlayerReceiveDebuff()
    {
        ownerOfDebuff.playerMovement.canMove = false;
        //Play big FX
    }

    public override void ApplyEffect()
    {
        ownerOfDebuff.playerMovement.canMove = false;
        //Play small FX
    }

    public override void RemoveEffect()
    {
        ownerOfDebuff.playerMovement.canMove = true;
        base.RemoveEffect();
    }
}
