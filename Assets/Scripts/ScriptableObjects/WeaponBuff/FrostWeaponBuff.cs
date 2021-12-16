using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuffWeapon Data/Frost Buff")]
public class FrostWeaponBuff : SO_BuffWeapon
{
    public int duration = 1;

    public override void ApplyWeaponBuff(Player playerAffect, Player playerAttacking)
    {
        playerAffect._debuffList.Add(new FreezeDebuff(duration, playerAffect));
    }
}
