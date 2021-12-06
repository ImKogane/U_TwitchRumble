using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuffWeapon Data")]
public abstract class SO_BuffWeapon : SO_Choice
{
    public virtual  void ApplyWeaponBuff(Player playerAffect, Player playerAttacking)
    {
        
    }
}
