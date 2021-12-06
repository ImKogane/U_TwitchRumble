using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuffWeapon Data/Fire Buff")]
public class FireWeaponBuff : SO_BuffWeapon
{
    public int duration = 1;
    public int dotDamages = 15;

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