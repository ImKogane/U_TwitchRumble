using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeaponBuff : WeaponBuff
{
    public int dotDamages = 15;
    public int duration = 1;

    public override void ApplyWeaponBuff(Player playerAffect, Player playerAttacking)
    {
        foreach (var debuff in playerAffect.debuffList)
        {
            if (debuff is BurningDebuff)
            {
                debuff.duration = duration;
                return;
            }
        }
        
        playerAffect.debuffList.Add(new BurningDebuff(duration, playerAffect, dotDamages));
    }

}
