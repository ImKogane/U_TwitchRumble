using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuffWeapon Data/Fire Buff")]
public class FireWeaponBuff : SO_BuffWeapon
{
    public int duration = 1;

    public override void ApplyWeaponBuff(Player playerAffect, Player playerAttacking)
    {
        foreach (var debuff in playerAffect._debuffList)
        {
            if (debuff is BurningDebuff)
            {
                debuff._duration = duration;
                return;
            }
        }

        playerAffect._debuffList.Add(new BurningDebuff(duration, playerAffect));
    }
}