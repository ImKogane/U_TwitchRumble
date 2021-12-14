using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/BuffWeapon Data")]
[System.Serializable]
public abstract class SO_BuffWeapon : SO_Choice
{
    public EnumClass.WeaponBuffType weaponBuffType;
    
    public virtual  void ApplyWeaponBuff(Player playerAffect, Player playerAttacking)
    {
        
    }
}
