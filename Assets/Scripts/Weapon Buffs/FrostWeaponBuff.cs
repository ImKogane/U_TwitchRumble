using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostWeaponBuff : WeaponBuff
{
    public int duration = 1;
    
    public override void ApplyWeaponBuff(Player playerAffect, Player playerAttacking)
    {
        playerAffect.debuffList.Add(new FreezeDebuff(duration, playerAffect));
    }
}
